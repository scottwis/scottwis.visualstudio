// NavigationTargetList.cs
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

using System.Collections;
using System.Collections.Generic;

namespace scottwis
{
    class NavigationTargetList : Dictionary<string, NavigationTargetInfo>
    {
        static readonly NavigationTargetList s_default = new NavigationTargetList {
            {"e", "Errors", "the error list", NavigationMode.Errors},
            {"1", "FindResults1", "the find results 1 window", NavigationMode.FindResults1},
            {"2", "FindResults2", "the find results 2 window", NavigationMode.FindResults2},
            {"s", "FindSymbolsResults", "resharper find usages results", NavigationMode.FindSymbolsResults},
            {"b", "BookMarks", "bookmarks list", NavigationMode.BookMarks},
            {"n", "StickyNotes", "sticky notes", NavigationMode.StickyNotes},
            {"t", "Tasks", "task list", NavigationMode.Tasks}
        };

        public static NavigationTargetList Default
        {
            get { return s_default; }
        }

        private void Add(string key, string name, string description, NavigationMode mode)
        {
            var x = new NavigationTargetInfo(key, name, description, mode);
            Add(x.Name, x);
        }
        public bool Execute(string name, ref NavigationMode target)
        {
            NavigationTargetInfo info;
            if (TryGetValue(name, out info)) {
                target = info.Mode;
                return true;
            }
            return false;
        }
    }
}
