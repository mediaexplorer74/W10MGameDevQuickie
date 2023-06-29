using System.Collections.Generic;

namespace FbonizziMonoGame.Assets.Abstractions
{
    /// <summary>
    /// It defines an importer of sprites
    /// </summary>
    public interface ISpriteImporter
    {
        /// <summary>
        /// Generates a dictionary of sprite name, sprite description given a file
        /// </summary>
        /// <param name="spriteSheetDescriptionFilePath"></param>
        /// <returns></returns>
        IDictionary<string, SpriteDescription> Import(string spriteSheetDescriptionFilePath);
    }
}
