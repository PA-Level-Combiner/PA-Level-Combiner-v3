using PALC.Main.Models.Combiners._20_4_4.LevelComponents;
using System.IO;

namespace PALC.Main.Models.Combiners._20_4_4;

public class LevelFolder
{
    public Level level;
    public string audioPath;
    public Metadata metadata;

    public LevelFolder(string levelFolderPath)
    {
        level = Level.FromFile(Path.Join(levelFolderPath, Level.defaultFileName));

        audioPath = Path.Combine(levelFolderPath, Audio.defaultFileName);
        if (!File.Exists(audioPath))
            throw new FileNotFoundException(null, fileName: audioPath);

        metadata = Metadata.FromFile(Path.Combine(levelFolderPath, Metadata.defaultFileName));
    }
}
