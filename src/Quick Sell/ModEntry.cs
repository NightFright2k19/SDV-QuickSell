﻿using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Quick_Sell
{
    public class ModEntry : Mod
    {
        public static Mod Instance;

        public static IModHelper Helper;

        public static ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            Instance = this;

            Helper = helper;

            Config = Helper.ReadConfig<ModConfig>();

            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            //if (!Context.IsPlayerFree)
            //    return;

            //if (!Game1.displayHUD)
            //    return;

            if (e.Button == Config.SellKey)
                this.OnSellButtonPressed(sender, e);
        }

        private void OnSellButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            Item item = ModPlayer.GetHoveredItem();

            if (item == null)
            {
                ModLogger.Debug("Item was null.");
                return;
            }

            ModLogger.Debug($"{Game1.player.Name} pressed {e.Button} and has selected {item}.");

            if (Config.CheckIfItemsCanBeShipped == true && ModPlayer.CheckIfItemCanBeShipped(item) == false)
            {
                ModLogger.Debug("Item was null or couldn't be shipped.");
                return;
            }

            ModPlayer.AddItemToPlayerShippingBin(item);

            ModPlayer.RemoveItemFromPlayerInventory(item);

            ModUtils.SendHUDMessage($"Sent {item.Stack} {item.DisplayName} to the Shipping Bin!");

            Game1.playSound("Ship");
        }
    }
}