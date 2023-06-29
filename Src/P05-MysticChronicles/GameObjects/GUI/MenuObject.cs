using System;
using System.Collections.Generic;
using GameManager.Library.Game.GameObjects.Characters;
using GameManager.Library.Game.GameStates;
using Microsoft.Xna.Framework;

//using MODEXngine.Library.Engine.GameObjects.GUI;
//using MODEXngine.Library.Engine.Objects.Common;
//using MODEXngine.Library.Engine.Objects.Element;
//using MODEXngine.Library.Engine.Objects.Element.Static;

namespace GameManager.Library.Game.GameObjects.GUI
{
    public class MenuObject : BaseGUIObject
    {
        static class _container { }

        public /*override*/ Tuple<List<BaseGraphicElement>, List<StaticText>> LoadContent()
        {
            var graphicElements = new List<BaseGraphicElement> 
            { 
                //new BackgroundImage(/*_container*/default, 
                //"UI/MainMenu") 
            };

            var textElements = new List<StaticText>
            {
                AddText(/*_container.Font*/default, 
                "Press any key to continue", 
                Color.White, 1, 
                //MODEXngine.Library.Engine.Enums.TextAlignment.HORIZONTALLY_AND_VERTICALLY_CENTERED
                default)
            };

            return new Tuple<List<BaseGraphicElement>, List<StaticText>>(graphicElements, textElements);
        }

        private StaticText AddText(object font, string v1, Color white, int v2, object v3)
        {
            throw new NotImplementedException();
        }

       
        public MenuObject(ElementContainer container) : base(container)
        {
            //
        }
    }
}