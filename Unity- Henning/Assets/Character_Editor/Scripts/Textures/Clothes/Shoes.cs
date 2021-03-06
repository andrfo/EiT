﻿namespace CharacterEditor
{
    namespace Textures
    {
        public class Shoes : AbstractTexture
        {
            public Shoes(ITextureLoader loader, int x, int y, int width, int height,
                string characterRace) :
                base(loader, x, y, width, height, characterRace, 23, TextureType.Shoe) {
            }

            public override string GetFolderPath() {
                return GetFolderPath(CharacterRace);
            }

            public static string GetFolderPath(string characterRace) {
                return "Assets/Character_Editor/Textures/Character/" + characterRace + "/Clothes/Shoe";
            }
        }
    }
}
