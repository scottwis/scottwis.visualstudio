// VisualStudio.cs
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
using EnvDTE;
using EnvDTE80;
using Extensibility;

namespace scottwis
{
    public class VisualStudio : IDTExtensibility2, IDTCommandTarget
    {
        DTE2 m_application;
        AddIn m_addIn;
        NavigationMode m_navMode = NavigationMode.FindSymbolsResults;

        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            m_application = (DTE2) application;
            m_addIn = (AddIn) addInInst;

            if (connectMode == ext_ConnectMode.ext_cm_UISetup) {
                CreateCommands();
            }
        }

        void CreateCommands()
        {
            var commands = m_application.Commands;

            CreateRegisterCommands(commands);
            CreateNavigationModeCommands(commands);
            CreateNavigationCommands(commands);
        }

        void CreateNavigationCommands(Commands commands)
        {
            var previousCmd = commands.AddNamedCommand(
                m_addIn,
                "GotoPrevious",
                "goto previous location",
                "Goto the previous location in the current navigation list.",
                false,
                0,
                vsCommandDisabledFlagsValue:
                    (int) vsCommandStatus.vsCommandStatusEnabled 
                    | (int) vsCommandStatus.vsCommandStatusSupported
            );

            previousCmd.Bindings = "Global::Alt+Left Arrow";
            
            var nextCmd = commands.AddNamedCommand(
                m_addIn,
                "GotoNext",
                "goto next location",
                "Goto the next location in the current navigation list.",
                false,
                0,
                vsCommandDisabledFlagsValue:
                    (int) vsCommandStatus.vsCommandStatusEnabled 
                    | (int) vsCommandStatus.vsCommandStatusSupported
            );

            nextCmd.Bindings = "Global::Alt+Right Arrow";
        }

        void CreateNavigationModeCommands(Commands commands)
        {
            foreach (var item in NavigationTargetList.Default.Values) {
                var cmd = commands.AddNamedCommand(
                    m_addIn,
                    string.Format("Navigate{0}", item.Name),
                    string.Format("Navigate {0}", item.Name),
                    string.Format("Change the navigation mode to '{0}'", item.Description),
                    false,
                    0,
                    vsCommandDisabledFlagsValue:
                        (int) vsCommandStatus.vsCommandStatusEnabled | (int) vsCommandStatus.vsCommandStatusSupported
                    );

                cmd.Bindings = new object[] {
                    string.Format("Global::ALT+N,ALT+{0}", item.Key),
                    string.Format("Global::ALT+N,{0}", item.Key)
                };
            }
        }

        void CreateRegisterCommands(Commands commands)
        {
            foreach (var item in EmacsRegisterList.Default.Values) {
                var copyCmdName = string.Format("CopyRegister{0}", item.Name);

                var copyCmd = commands.AddNamedCommand(
                    m_addIn,
                    copyCmdName,
                    string.Format("Copy Register {0}", item.Name),
                    string.Format("Copy Register {0}", item.Name),
                    false,
                    0,
                    vsCommandDisabledFlagsValue:
                        (int) vsCommandStatus.vsCommandStatusEnabled | (int) vsCommandStatus.vsCommandStatusSupported
                    );

                var pasteCmd = commands.AddNamedCommand(
                    m_addIn,
                    string.Format("PasteRegister{0}", item.Name),
                    string.Format("Paste Register {0}", item.Name),
                    string.Format("Paste Register {0}", item.Name),
                    false,
                    0,
                    vsCommandDisabledFlagsValue:
                        (int) vsCommandStatus.vsCommandStatusEnabled | (int) vsCommandStatus.vsCommandStatusSupported
                    );

                if (item.Key != null) {
                    copyCmd.Bindings = new object[] {
                        String.Format("Global::ALT+C, ALT+{0}", item.Key),
                        String.Format("Global::ALT+C, {0}", item.Key)
                    };

                    pasteCmd.Bindings = new object[] {
                        String.Format("Global::ALT+V, ALT+{0}", item.Key),
                        String.Format("Global::ALT+V, {0}", item.Key)
                    };
                }
            }
        }

        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        public void OnAddInsUpdate(ref Array custom)
        {
        }

        public void OnStartupComplete(ref Array custom)
        {
        }

        public void OnBeginShutdown(ref Array custom)
        {
        }


        public void QueryStatus(string cmdName, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText)
        {
            if (cmdName.StartsWith("scottwis.VisualStudio")) {
                statusOption = vsCommandStatus.vsCommandStatusEnabled | vsCommandStatus.vsCommandStatusSupported;
            }
        }

        public void Exec(string cmdName, vsCommandExecOption executeOption, ref object variantIn, ref object variantOut, ref bool handled)
        {
            if (cmdName.StartsWith("scottwis.VisualStudio.") && ! handled) {
                cmdName = cmdName.Substring("scottwis.VisualStudio.".Length);
                if (cmdName.StartsWith("Navigate")) {
                    handled = NavigationTargetList.Default.Execute(cmdName.Substring("Navigate".Length), ref m_navMode);
                }
                else if (cmdName.StartsWith("CopyRegister")) {
                    handled = ExecuteCopy(cmdName.Substring("CopyRegister".Length));
                }
                else  if (cmdName.StartsWith("PasteRegister")) {
                    handled = ExecutePaste(cmdName.Substring("PasteRegister".Length));
                }
                else if (cmdName.Equals("GotoPrevious")) {
                    handled = ExecuteGotoPrevious();
                }
                else if (cmdName.Equals("GotoNext")) {
                    handled = ExecuteGotoNext();
                }

            }
        }

        bool ExecuteCopy(string name)
        {
            EmacsRegisterInfo info;
            if (EmacsRegisterList.Default.TryGetValue(name, out info)) {
                var selection = m_application.ActiveDocument != null ? m_application.ActiveDocument.Selection as TextSelection : null;
                if (selection != null && !selection.IsEmpty) {
                    info.Value = selection.Text;
                }
                return true;
            }
            return false;
        }

        bool ExecutePaste(string name)
        {
            EmacsRegisterInfo info;
            if (EmacsRegisterList.Default.TryGetValue(name, out info))
            {
                var selection = m_application.ActiveDocument != null ? m_application.ActiveDocument.Selection as TextSelection : null;
                if (selection != null && info.Value != null) {
                    selection.Text = info.Value;
                }
                return true;
            }
            return false;
        }

        bool ExecuteGotoNext()
        {
            switch (m_navMode) {
                case NavigationMode.Errors:
                    m_application.ExecuteCommand("View.NextError");
                    return true;
                case NavigationMode.FindResults1:
                    m_application.ExecuteCommand("Edit.GotoFindResults1NextLocation");
                    return true;
                case NavigationMode.FindResults2:
                    m_application.ExecuteCommand("Edit.GotoFindResults2NextLocation");
                    return true;
                case NavigationMode.FindSymbolsResults:
                    m_application.ExecuteCommand("ReSharper.ReSharper_ResultList_GoToNextLocation");
                    return true;
                case NavigationMode.BookMarks:
                    m_application.ExecuteCommand("Edit.NextBookmark");
                    return true;
                case NavigationMode.StickyNotes:
                    //TODO: Implement this
                    return true;
                case NavigationMode.Tasks:
                    m_application.ExecuteCommand("View.NextTask");
                    return true;
            }
            return false;
        }

        bool ExecuteGotoPrevious()
        {
            switch (m_navMode) {
                case NavigationMode.Errors:
                    m_application.ExecuteCommand("View.PreviousError");
                    return true;
                case NavigationMode.FindResults1:
                    m_application.ExecuteCommand("Edit.GotoFindResults1PreviousLocation");
                    return true;
                case NavigationMode.FindResults2:
                    m_application.ExecuteCommand("Edit.GotoFindResults2PreviousLocation");
                    return true;
                case NavigationMode.FindSymbolsResults:
                    m_application.ExecuteCommand("ReSharper.ReSharper_ResultList_GoToPrevLocation");
                    return true;
                case NavigationMode.BookMarks:
                    m_application.ExecuteCommand("Edit.PreviousBookmark");
                    return true;
                case NavigationMode.StickyNotes:
                    //TODO: Implement this
                    return true;
                case NavigationMode.Tasks:
                    m_application.ExecuteCommand("View.PreviousTask");
                    return true;
            }
            return false;
        }
    }
}
