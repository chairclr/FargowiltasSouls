﻿using FargowiltasSouls.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace FargowiltasSouls.Content.UI.Elements
{
    public class SoulTogglerButton : UIState
    {
        public UIImage Icon;
        public FargoUIHoverTextImageButton IconHighlight;
        public UIImage IconFlash;
        public UIOncomingMutant OncomingMutant;

        public override void OnActivate()
        {
            const int x = 570;
            const int y = 275;

            IconFlash = new UIImage(FargoUIManager.SoulTogglerButton_MouseOverTexture);
            IconFlash.Left.Set(x, 0);
            IconFlash.Top.Set(y, 0);
            Append(IconFlash);

            Icon = new UIImage(FargoUIManager.SoulTogglerButtonTexture);
            Icon.Left.Set(x, 0); //26
            Icon.Top.Set(y, 0); //300
            Append(Icon);

            IconHighlight = new FargoUIHoverTextImageButton(FargoUIManager.SoulTogglerButton_MouseOverTexture, FargoSoulsUtil.IsChinese() ? "设置饰品效果" : "Configure Accessory Effects");
            IconHighlight.Left.Set(0, 0);
            IconHighlight.Top.Set(0, 0);
            IconHighlight.SetVisibility(1f, 0);
            IconHighlight.OnLeftClick += IconHighlight_OnClick;
            Icon.Append(IconHighlight);

            OncomingMutant = new UIOncomingMutant(FargoUIManager.OncomingMutantTexture.Value, FargoUIManager.OncomingMutantAuraTexture.Value, FargoSoulsUtil.IsChinese() ? "永恒模式已开启" : "Eternity Mode is enabled", FargoSoulsUtil.IsChinese() ? "受虐模式已开启" : "Masochist Mode is enabled");
            OncomingMutant.Left.Set(610, 0);
            OncomingMutant.Top.Set(250, 0);
            Append(OncomingMutant);

            base.OnActivate();
        }

        private void IconHighlight_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!Main.playerInventory)
            {
                return;
            }
            
            FargoUIManager.ToggleSoulToggler();
            Main.LocalPlayer.FargoSouls().HasClickedWrench = true;
        }
        

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.playerInventory)
            {
                //base.Draw(spriteBatch);

                Icon.Draw(spriteBatch);
                IconHighlight.Draw(spriteBatch);
                OncomingMutant.Draw(spriteBatch);
                if (!Main.LocalPlayer.FargoSouls().HasClickedWrench && Main.GlobalTimeWrappedHourly % 1f < 0.5f)
                    IconFlash.Draw(spriteBatch);
            }
        }
    }
}
