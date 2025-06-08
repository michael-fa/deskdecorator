using deskdecorator;

public static class Language
{
    public static string Get(string key)
    {
        if (!Strings.ContainsKey(key)) return key;
        return Strings[key][Program.gLanguage];
    }

    private static readonly Dictionary<string, string[]> Strings = new()
    {
        ["Error"] = new[] {"Error", "Fehler"},
        ["error"] = new[] {"error", "fehler"},
        ["Notice"] = new[] {"Notice", "Hinweis"},
        
        //form1
        ["form1_001"] = new[] { "Error while creating context menu:", "Fehler beim Einrichten des Explorer-Menüs:" },
        ["form1_002"] = new[] { "Please select an element in the selection first.", "Bitte zuerst ein Element im Dropdown auswählen." },
        ["form1_003"] = new[] { "The element could not be found (anymore).", "Das gewählte Element konnte nicht gefunden werden." },
        ["form1_004"] = new[] { "No element selected!", "Es ist kein Element ausgewählt." },
        ["form1_005"] = new[] { "Element not found!", "Element konnte nicht gefunden werden." },
        ["form1_006"] = new[] { "Are you sure you want to delete this label?", "Möchtest du dieses Label wirklich löschen?" },
        ["form1_007"] = new[] { "Confirm deletion", "Löschen bestätigen" },
        ["form1_008"] = new[] { "Copyright © 2025 Michael Fanter\nwww.github.com/michael-fa", "Programmiert von Michael Fanter\nCopyright © 2025\nwww.github.com/michael-fa" },
        
        
        ["crlabel_btn_dynamic"] = new[] {"Dynamic", "Dynamisch"},
        ["crlabel_lbl_description"] = new[] {"Label description", "Beschreibung"},
        ["crlabel_lbl_text"] = new[] {"Label text", "Inhalt"},
        ["crlabel_lbl_setpos"] = new[] {"Set position", "Position setzen"},
        ["crlabel_btn_font"] = new[] {"Font", "Schriftart"},
        ["crlabel_title"] = new[] {"DeskDecorator - Create Label", "DeskDecorator - Text erstellen"},
        ["create_here"] = new[] {"Create here", "Hier erstellen"},
        ["crlabel_editlabelversion"] = new[] { "DeskDecorator - Edit label", "DeskDecorator - Text bearbeiten" },
        ["Help"] = new[] { "Help", "Hilfe" },
        ["helptext"] = new[] { "Work in progress", "Hier könnte Hilfe stehen 😄" },
        ["crlabel_001"] = new[] { "is already taken, please use another name.", "ist bereits vergeben. Bitte wähle einen anderen Namen." },
        ["crlabel_002"] = new[] { "The entered name contains invalid characters and/or letters.", "Der Name enthält ungültige Zeichen für Dateinamen." },
        ["crlabel_invalidname"] = new[] { "Invalid name", "Ungültiger Name" },
        
        
        ["settings_001"] = new[] { "Run Deskdecorator on windows startup", "Beim Start von Windows ausführen" },
        ["settings_002"] = new[] { "DeskDecorator - Settings", "DeskDecorator - Einstellungen" },
        
        ["systray_noitemsel"] = new[] {"No item selected", "Keine auswahl"},
        ["systray_crnwlbl"] = new[] {"Create Label", "Text erstellen"},
        ["systray_edelmt"] = new[] {"Edit Label", "Text bearbeiten"},
        ["systray_delelement"] = new[] {"Delete Label", "Text löschen"},
        ["systray_settings"] = new[] {"Settings", "Einstellungen"},
        ["systray_credits"] = new[] {"Credits", "Über"},
        ["systray_close"] = new[] {"Close", "Schließen"},
        

        ["autostart"] = new[] { "Autostart", "Autostart" },
        ["add_label"] = new[] { "Add Label", "Beschriftung hinzufügen" },
        ["remove"] = new[] { "Remove", "Entfernen" },
        ["save"] = new[] { "Save", "Speichern" },
        ["confirm_delete"] = new[] { "Are you sure?", "Bist du sicher?" },
        ["ok"] = new[] { "OK", "OK" },
        ["cancel"] = new[] { "Cancel", "Abbrechen" },
        ["settings"] = new[] { "Settings", "Einstellungen" },
        ["language"] = new[] { "language", "sprache" },
        ["Language"] = new[] { "Language", "Sprache" }
        // ...
    };
}
