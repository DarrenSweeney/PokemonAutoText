using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonAutoText
{
    class TextBox
    {
        private Rectangle textBox;
        private Texture2D debugBox;
        private SpriteFont font;

        private String text;
        private Vector2 position;
        private String parsedText;
        private Double typeTextLength;
        private int delayInMilliseconds;
        private float scale;
        private String typedText;

        public bool isDoneDrawing;
        public bool startedDrawing;

        public TextBox(ContentManager content, String newText, Vector2 newPosition, float newScale, int width, int height, int letterDelay)
        {
            position = newPosition;
            textBox = new Rectangle((int)position.X, (int)position.Y, width, height);
            debugBox = content.Load<Texture2D>("textBox");
            font = content.Load<SpriteFont>("font");
            text = newText;
            scale = newScale;

            parsedText = parseText(text);
            delayInMilliseconds = letterDelay;

            isDoneDrawing = false;
            startedDrawing = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!isDoneDrawing)
            {
                if (delayInMilliseconds == 0)
                {
                    typedText = parsedText;
                    isDoneDrawing = true;
                }

                else if (typeTextLength < parsedText.Length)
                {
                    if (startedDrawing == true)
                    {
                        typeTextLength = typeTextLength + gameTime.ElapsedGameTime.TotalMilliseconds / delayInMilliseconds;
                    }

                    if (typeTextLength >= parsedText.Length)
                    {
                        typeTextLength = parsedText.Length;
                        isDoneDrawing = true;
                    }
                    typedText = parsedText.Substring(0, (int)typeTextLength);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(debugBox, textBox, Color.Red);
            spriteBatch.DrawString(font, typedText, new Vector2(textBox.X, textBox.Y), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        private String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (font.MeasureString(line + word).Length() > textBox.Width * 1f)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }
                line = line + word + ' ';
            }
            return returnString + line;
        }
    }
}
