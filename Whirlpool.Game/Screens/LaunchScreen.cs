using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System.Collections.Generic;
using System.Drawing;
using UI = Whirlpool.Core.UI;

namespace Whirlpool.Game.Screens
{
    public class LaunchScreen : Screen
    {
        public override void Init()
        {
            Size size = MainGame.windowSize;
            // TODO: make this a separate file for easy changing
            AddComponents(new List<RenderComponent>() {
                new UI.Label()
                {
                    text = "OSLO",
                    font = new UI.Font("Content\\Fonts\\Heebo-Bold.ttf", Color4.White, 128, 2),
                    position = new Vector2(size.Width / 2, size.Height / 2),
                    centered = true
                },
                new UI.Label()
                {
                    text = "start",
                    font = new UI.Font("Content\\Fonts\\Montserrat-Regular.ttf", Color4.White, 32, 2),
                    position = new Vector2(size.Width / 2, size.Height / 2 + 75),
                    centered = true
                },
                //new UI.Button()
                //{
                //    text = "start",
                //    font = new UI.Font("Content\\Fonts\\Montserrat-Regular.ttf", Color4.White, 48, 0),
                //    position = new Vector2(size.Width / 2, size.Height / 2 + 75),
                //    size = new Vector2(200, 200),
                //    centered = true
                //},
                //new UI.Checkbox()
                //{
                //    isChecked = false,
                //    position = new Vector2(size.Width / 2 - 32, size.Height / 2 + 100 - 32),
                //    checkboxTexture = Texture.FromFile("Content\\checkbox.png"),
                //    checkboxCheckedTexture = Texture.FromFile("Content\\checkboxChecked.png")
                //},
                new UI.Label()
                {
                    text = "made with ❤ by agthrs",
                    font = new UI.Font("Content\\Fonts\\Montserrat-Regular.ttf", Color4.White, 24, 0),
                    position = new Vector2(size.Width / 2, size.Height - 35),
                    centered = true
                },
                //new UI.SlicedSprite()
                //{
                //    size = new Vector2(472, 128),
                //    position = new Vector2(size.Width / 2 - 128, size.Height / 2 + 32),
                //    spriteLoc = "Content\\shadow.png"
                //},
                //new Render.Model()
                //{
                //    objName = "Content\\octahedron.obj",
                //    size = new Vector3(0.2f, .2f, .2f),
                //    position = new Vector3(0, 0, -4.0f)
                //},
                //new UI.Notification()
                //{
                //    text = "Test",
                //    font = new UI.Font("Content\\Fonts\\Montserrat-Regular.ttf", Color4.White, 48, 0),
                //    position = new Vector2(size.Width / 2, size.Height / 2 + 75),
                //    size = new Vector2(200, 200)
                //},
            });
            base.Init();
        }

        public override void Render()
        {
            Size size = MainGame.windowSize;
            BaseRenderer.RenderQuad(new Vector2(0, 0), new Vector2(size.Width, size.Height), "blank", new Color4(255, 99, 71, 255));
            base.Render();
        }

        public override void Update()
        {
            // TODO: find some method of anchoring rendercomponents (e.g. centre of screen on resize)
            // testLabel.position = new Vector2(Size.Width / 2, Size.Height / 2);
            base.Update();
        }
    }
}
