    Defines an add-in for Visual Studio designed to replace my personal
    macros. Visual Studio 11 dropped support for Visual Studio macros.
    
    It is meant to be used in conjection with KeyAndMenu.vssettings, which
    defines my personal keyboard scheme, tab bar configuration, color scheme
    and font selection.
    
    Both the keyboard scheme and the addin have a depenency on Resharper.
    
    It has only been tested with the Visual Studio 11 Beta.
    
    The add-in provides 2 main features:
    
    1. A flexible "navigation system" bound to the "Alt+Left Arrow" and
    "Alt+Right Arrow" keys. The addin can be put into different "navigation
    modes", which basically changes the list that the navigation keys operate
    on. Currently the following lists can be navigated:
    
        a. The find 1 window results
        b. The find 2 window results.
        c. The resharper "find usages" results.
        d. The VS task list.
        e. The VS book mark list.
        f. The errors list.
    
    Some support is also in place for navigating the list of "sticky notes" in
    a file. It's stubbed out at the moment, however, because there currently
    is no VS 11 version of the "Sticky Notes For Code" add-in. I have not
    tried the VS2010 version yet, it may work.
    
    2. Emacs style "register macros". you can hit "Alt+C, Key" where "Key" is
    pretty much any "simple key" on a standard US english keyboard layout to
    copy the current selection into the "register" named by the key. You can
    similarly paste the value with the associated register by hitting
    "Alt+V, Key". You can also use "Alt+V, Alt+Key" and "Alt+C, Alt+Key" for
    convience. In other words, it doesn't matter if you hold the "Alt" key
    down for the second key in the cord.
    
    The VS customization file also defines several other useful keystrokes:
    
    1. Ctrl+Alt+Left and Ctrl+Alt+Right
    
    These navigate through the last "editor position". Where as "Alt+Left"
    will take you to the last "Compiler Error Location" (or other list
    location depending on the current navigation mode"), "Ctrl+Alt+Left" will
    take you to the previous position your cursor was at, independent of your
    position within any navigation lists.
    
    2. Ctrl+/
    
    This binds to the "Goto Command Line" command. It sets focus to the find
    combo and inserts a ">" as the first character, so that you can run any
    visual studio command.
    
    3. Ctrl+O
    
    This binds to the resharper "Open file command". It works similarly to the
    "Open" comamnd in Source Insight when the file list window is not
    docked. It shows a simple popup that you can use to "word wheel" to a file
    in the current solution you wish to open.
    
    4. F7, F8
    
    These bind to the resharper "Goto Symbol" and "Goto File Member" commands.
    They work similarly to the corresponding commands in Source Insight. They
    show simple popups that can be used to "word wheel" to any symbol in the
    current solution (or referenced assemblies) or any symbol in the current
    file.
    
    5. Alt+Enter
    
    This binds to the resharper "quick fix" command. It can be used to fix
    issues highlighted by resharper.
    
    6. Ctrl+,
    
    This brings up the resharper "navigate" command. It brings up a menu of
    different places you can navigate to with respect to the symbol under the
    cursor. It's basically a more powerful version of "Goto Definition".
    
    
    7. Ctrl+D
    
    This binds to the resharper "Goto Definition" command.
    
    8. Alt+B
    
    This binds to the resharper "goto base" command. It will navigate to a
    base class, or base method associated with the symbol underneath the
    cursor.
    
    9. Alt+D
    
    This binds to the resharper "goto inheritors" command. When applied to a
    class or interface it will show a list of all implementing or inheritng
    types, allowing you to navigate to them. When applied to a method it will
    show a list of all implementations or overides of that method, allowing
    you to navigate to them.
    
    10. Alt+G
    
    This binds to the resharper "generate" command. It displays a list of
    various types of code that can be generated in the current context. The
    list includes things like "Missing Methods" and "Overriding methods", plus
    a bunch of other useful stuff.
    
    11. Alt+PgDwn / Alt+PgUp
    
    These navigate between the resharper "highlights" in the current file.
    
    12. Alt+K,Alt+K
    
    This code runs the resharper "cleanup code silently command".
    
    13. Ctrl+\
    
    This is bound to the resharper "find usages" command. It finds references
    to the symbol under the cursor.  You can navigate between the results by
    hitting Alt+N, S and then using Alt+Left and Alt+Right.

    14. Ctrl+P

    This is bound to the resharper "show parameter info" command. It allows you
    to view intellisence parameter info for the current argument you are 
    specifying at a method call site.

    14. Ctrl+T, Ctrl+R, Ctrl+E

    These are bound to "view task list", "view book marks", and 
    "view error list" commands.

    15. Alt+Up, Alt+Down

    These are bound to the resharper "goto previous method" / "goto next method" commands.
    They allow you to navigate quickly between methods in a file.


