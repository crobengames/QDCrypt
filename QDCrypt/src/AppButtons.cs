using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QDCrypt {

    public enum BtnState {
        Leave,
        Hovered
    }


    public enum BtnType {
        Encrypt,
        Decrypt,
        Clear,
        About
    }



    class AppButtons {

        public static void UpdateButtonImage(Button button, BtnType type, BtnState state) {

            switch (state) {
                default:
                case BtnState.Leave:
                    SetButtonImage(button, GetLeaveImage(type));
                    break;
                case BtnState.Hovered:
                    SetButtonImage(button, GetHoveredImage(type));
                    break;
            }
        }


        private static void SetButtonImage(Button button, Image image) => button.Image = image;


        private static Image GetHoveredImage(BtnType type) {

            switch (type) {
                case BtnType.Encrypt:
                    return Properties.Resources.encrypthover;
                case BtnType.Decrypt:
                    return Properties.Resources.decrypthover;
                case BtnType.Clear:
                    return Properties.Resources.clearhover;
                case BtnType.About:
                    return Properties.Resources.abouthover;
            }

            return null;
        }


        private static Image GetLeaveImage(BtnType type) {

            switch (type) {
                case BtnType.Encrypt:
                    return Properties.Resources.encrypt;
                case BtnType.Decrypt:
                    return Properties.Resources.decrypt;
                case BtnType.Clear:
                    return Properties.Resources.clear;
                case BtnType.About:
                    return Properties.Resources.about;
            }

            return null;
        }

    }
}
