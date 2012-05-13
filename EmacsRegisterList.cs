// EmacsRegisterList.cs
//  
// Author:
//     Scott Wisniewski <scott@scottdw2.com>
//  
// Copyright (c) 2012 Scott Wisniewski
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace scottwis
{
    sealed class EmacsRegisterList : Dictionary<String, EmacsRegisterInfo>
    {
        static readonly EmacsRegisterList s_default = new EmacsRegisterList {
            {"BackQuote", "`"},
            '0'.UpTo('9'),
            {"Minus", "-"},
            {"Equals","="},
            'a'.UpTo('z'),
            {"LeftSquareBrace", "["},
            {"RightSquareBrace", "]"},
            {"BackSlash", "\\"},
            {"SemiColon", ";"},
            {"Quote", "'"},
            "Comma",
            {"Dot", "."},
            {"ForwardSlash","/"},
            {"Add", "+"},
            {"Multiply", "*"}
        };

        public static EmacsRegisterList Default
        {
            get { return s_default; }
        }

        public void Add(char c)
        {
            var name = c.ToString().ToUpper();
            Add(name, new EmacsRegisterInfo(c.ToString(), name));
        }

        public void Add(IEnumerable<char> items)
        {
            items.Foreach(this.Add);
        }

        public void Add(string name, string key)
        {
            Add(name, new EmacsRegisterInfo(key, name));
        }

        public void Add(string name)
        {
            Add(name, (string)null);
        }

    }
}
