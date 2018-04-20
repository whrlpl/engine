using OpenTK;
using OpenTK.Graphics;
using OpenTKTest.Core;
using OpenTKTest.Core.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Game.Screens
{
    public class LaunchScreen : Screen
    {
        public override void Init()
        {
            Size size = TDBRGame.windowSize;
            AddComponents(new List<RenderComponent>() {
                new UI.Label()
                {
                    text = "ASIMOV",
                    font = new UI.Font("Content\\Fonts\\Catamaran-Thin.ttf", Color4.White, 72, 2),
                    position = new Vector2(size.Width / 2, size.Height / 2),
                    centered = true
                },
                //new UI.Notification()
                //{
                //    text = "Test",
                //    font = new UI.Font("Content\\Fonts\\Catamaran-Bold.ttf", Color4.White, 48, 0),
                //    position = new Vector2(size.Width / 2, size.Height / 2 + 75),
                //    size = new Vector2(200, 200)
                //},
                new UI.Button()
                {
                    text = "start",
                    font = new UI.Font("Content\\Fonts\\Catamaran-Bold.ttf", Color4.White, 48, 0),
                    position = new Vector2(size.Width / 2, size.Height / 2 + 75),
                    size = new Vector2(200, 200),
                    centered = true
                },
                new UI.Checkbox()
                {
                    isChecked = false,
                    position = new Vector2(size.Width / 2 - 32, size.Height / 2 + 100 - 32),
                    checkboxTexture = Texture.FromFile("Content\\checkbox.png"),
                    checkboxCheckedTexture = Texture.FromFile("Content\\checkboxChecked.png")
                },
                new UI.Label()
                {
                    text = "made with ❤ by agthrs",
                    font = new UI.Font("Content\\Fonts\\Catamaran-Bold.ttf", Color4.White, 24, 0),
                    position = new Vector2(size.Width / 2, size.Height - 35),
                    centered = true
                },
                //new Render.Model()
                //{
                //    objName = "Content\\octahedron.obj",
                //    size = new Vector3(0.2f, .2f, .2f),
                //    position = new Vector3(0, 0, -4.0f)
                //}
            });
            base.Init();
        }


        public override void Update()
        {
            // TODO: find some method of anchoring rendercomponents (e.g. centre of screen on resize)
            // testLabel.position = new Vector2(Size.Width / 2, Size.Height / 2);
            base.Update();
        }
    }
}
