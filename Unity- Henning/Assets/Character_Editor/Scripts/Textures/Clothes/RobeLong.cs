﻿namespace CharacterEditor
{
    namespace Textures
    {
        public class RobeLong : AbstractTexture
        {

            public RobeLong(ITextureLoader loader, int x, int y, int width, int height,
                string characterRace) :
                base(loader, x, y, width, height, characterRace, 26, TextureType.RobeLong) {
            }

            public override void MoveNext() {
                if (SelectedTexture == textures.Length - 1 && textures.Length > 1) {
                    SelectedTexture = 1; //Skip empty robe
                }
                else {
                    SelectedTexture++;
                }
            }

            public override void MovePrev() {
                if (SelectedTexture == 1 && textures.Length > 1) {
                    SelectedTexture = textures.Length - 1; //Skip empty robe
                }
                else {
                    SelectedTexture--;
                }
            }

            public override string GetFolderPath() {
                return GetFolderPath(CharacterRace);
            }

            public static string GetFolderPath(string characterRace) {
                return "Assets/Character_Editor/Textures/Character/" + characterRace + "/Clothes/RobeLong";
            }
        }
    }
}
