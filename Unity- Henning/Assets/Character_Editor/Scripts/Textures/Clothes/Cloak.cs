﻿namespace CharacterEditor
{
    namespace Textures
    {
        public class Cloak : AbstractTexture
        {
            public Cloak(ITextureLoader loader, int x, int y, int width, int height,
                string characterRace) :
                base(loader, x, y, width, height, characterRace, 24, TextureType.Cloak) {
            }

            public override string GetFolderPath() {
                return GetFolderPath(CharacterRace);
            }

            public static string GetFolderPath(string characterRace) {
                return "Assets/Character_Editor/Textures/Character/" + characterRace + "/Clothes/Cloak";
            }
        }
    }
}
