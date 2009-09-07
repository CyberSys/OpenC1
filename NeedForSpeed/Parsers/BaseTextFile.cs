﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Carmageddon.Parsers
{
    abstract class BaseTextFile
    {
        StreamReader _file;

        public BaseTextFile(string filename)
        {
            _file = new StreamReader(filename);
        }

        public void CloseFile()
        {
            _file.Close();
        }

        public void SkipLines(int skip)
        {
            if (skip == 0) return;
            int count = 0;
            while (true)
            {
                string line = _file.ReadLine();
                if (!line.StartsWith("//") && line != "") count++; //ignore comment lines

                if (count == skip)
                    break;
            }
        }

        public string SkipLinesTillComment(string comment)
        {
            while (true)
            {
                string line = _file.ReadLine();
                if (line.Contains(comment))
                    return line;
            }
        }

        /// <summary>
        /// Return the next non-comment line, with comment stripped out
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string ReadLine()
        {
            while (true)
            {
                string line = _file.ReadLine();
                if (!line.StartsWith("//") && line != "")
                {
                    return line.Split(new string[] { "//" }, StringSplitOptions.None)[0].Trim();
                }
            }
        }

        public int ReadLineAsInt()
        {
            string line = ReadLine();
            return int.Parse(line);
        }

        public Vector3 ReadLineAsVector3()
        {
            return ReadLineAsVector3(true);
        }

        public Vector3 ReadLineAsVector3(bool scale)
        {
            string line = ReadLine();
            string[] tokens = line.Split(new char[] {',', '\t', ' '}, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert(tokens.Length == 3);
            Vector3 vec = new Vector3(float.Parse(tokens[0]), float.Parse(tokens[1]), float.Parse(tokens[2]));
            if (scale) vec *= GameVariables.Scale;
            return vec;
        }

        public Vector2 ReadLineAsVector2(bool scale)
        {
            string line = ReadLine();
            string[] tokens = line.Split(new char[] { ',', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert(tokens.Length == 2);
            Vector2 vec = new Vector2(float.Parse(tokens[0]), float.Parse(tokens[1]));
            if (scale) vec *= new Vector2(GameVariables.Scale.X, GameVariables.Scale.Y);
            return vec;
        }

        public Matrix ReadMatrix()
        {
            Matrix m = new Matrix();
            Vector3 v = ReadLineAsVector3(false);
            m.M11 = v.X;
            m.M12 = v.Y;
            m.M13 = v.Z;
            v = ReadLineAsVector3(false);
            m.M21 = v.X;
            m.M22 = v.Y;
            m.M23 = v.Z;
            v = ReadLineAsVector3(false);
            m.M31 = v.X;
            m.M32 = v.Y;
            m.M33 = v.Z;
            v = ReadLineAsVector3(false);
            m.M41 = v.X;
            m.M42 = v.Y;
            m.M43 = v.Z;
            m.M44 = 1;
            
            return m;
        }
    }
}
