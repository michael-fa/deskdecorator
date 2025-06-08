using System.Diagnostics;

namespace deskdecorator.DekstopEngine.DesktopElements
{
    public enum ELEMENT_TYPE
    {
        LABEL = 0
        //IMAGE = 1
    }

    internal static class Manager
    {
        public static List<Label> Labels = new();
        //public static List<ImageElement> Images = new();
        public static readonly string SaveDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saved_elements");

        public static void Label_Add(Label element)
        {
            if (!Labels.Contains(element))
                Labels.Add(element);
        }

        public static void Label_Remove(Label element)
        {
            Labels.Remove(element);
        }

        /*public static void Image_Add(ImageElement element)
        {
            if (!Images.Contains(element))
                Images.Add(element);
        }

        public static void Image_Remove(ImageElement element)
        {
            Images.Remove(element);
        }*/

        /// <summary>
        /// Erneuert alle dynamischen Texte (z. B. Platzhalter für Uhrzeit)
        /// </summary>
        public static void UpdateLabels()
        {
            foreach (var element in Labels)
            {
                if (element == null || element.Type != ELEMENT_TYPE.LABEL)
                    continue;

                try
                {
                    element.Update();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[ERROR] Update error in label '{element.Description}': {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Speichert alle geladenen Elemente dauerhaft ab.
        /// </summary>
        public static void SaveElements()
        {
            int count = 0;

            foreach (var label in Labels)
            {
                try
                {
                    label.Save();
                    count++;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[ERROR] Failed to save label '{label.Description}': {ex.Message}");
                }
            }

            /*foreach (var image in Images)
            {
                try
                {
                    image.Save();
                    count++;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[ERROR] Failed to save image '{image.Description}': {ex.Message}");
                }
            }*/

            Debug.WriteLine($"[INFO] Saved {count} elements.");
        }

        /// <summary>
        /// Lädt alle gespeicherten Elemente beim Start.
        /// </summary>
        public static void LoadElements()
        {
            if (!Directory.Exists(SaveDirectory))
                return;

            foreach (string file in Directory.GetFiles(SaveDirectory))
            {
                try
                {
                    var prop = new tools.Properties(file);

                    switch (prop.get("type"))
                    {
                        case "0": // Label
                            Debug.WriteLine($"[INFO] Loading label: {file}");

                            var label = new Label(
                                prop.get("description"),
                                prop.get("text"),
                                prop,
                                prop.get("dynamic").Contains("True")
                            );

                            Label_Add(label);

                            label.Position = new Point(
                                Convert.ToInt32(prop.get("positionX")),
                                Convert.ToInt32(prop.get("positionY"))
                            );

                            label.CreateControl(label.Position);
                            break;

                        /*case "1": // Image
                            Debug.WriteLine($"[INFO] Loading image: {file}");
                            var image = new ImageElement(
                                prop.get("description"),
                                prop.get("path"));
                            image.Position = new Point(
                                Convert.ToInt32(prop.get("positionX")),
                                Convert.ToInt32(prop.get("positionY")));
                            image.CreateControl(image.Position);
                            Image_Add(image);
                            break;
                        */



                        default:
                            Debug.WriteLine($"[WARNING] Unknown element type in file: {file}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[WARNING] Error loading element from {file}: {ex.Message}");
                }
            }
        }
    }
}
