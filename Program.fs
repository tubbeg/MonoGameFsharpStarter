
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

let buttonStateIsPressed (g : GamePadState) =
    g.Buttons.Back = ButtonState.Pressed 

let escapeIsPressed (k : KeyboardState) =
    Keys.Escape |> k.IsKeyDown

// doing this inside a method is appearently not ok for some magic reason
let divVector  (div : float32) (v : Vector2) =
    v / div

type MyGame () as this =
    inherit Game()
    let _graphics = new GraphicsDeviceManager (this)
    let mutable _spriteBatch = None
    let mutable _spriteFont = None
    let mutable _fontPos = None
    do
        this.Content.RootDirectory <- "Content"
        this.IsMouseVisible <- true

    override this.Initialize (): unit = 
            base.Initialize()

    override this.LoadContent (): unit = 
            base.LoadContent()
            _spriteBatch <- new SpriteBatch(this.GraphicsDevice) |> Some
            _spriteFont <- this.Content.Load<SpriteFont>("MyMenuFont2") |> Some
            let vp = _graphics.GraphicsDevice.Viewport
            let ch, cw =
                let w = vp.Width |> float32
                let h = vp.Height |> float32
                (h / 2.0f),(w / 2.0f)
            
            _fontPos <- new Vector2(ch,cw) |> Some

    override this.Update (gameTime: GameTime): unit = 
            base.Update(gameTime: GameTime)
            let pressedBack = GamePad.GetState PlayerIndex.One |> buttonStateIsPressed
            let pressedEscape = Keyboard.GetState() |> escapeIsPressed
            match pressedBack, pressedEscape with
            | true, _ -> this.Exit()
            | _ , true -> this.Exit()
            | _ -> ()

    override this.Draw (gameTime: GameTime): unit = 
            this.GraphicsDevice.Clear Color.CornflowerBlue
            let text = "hello hello hello"
            match _spriteBatch, _spriteFont, _fontPos with
            | Some sb, Some sf, Some fp ->
                sb.Begin()
                let fo = sf.MeasureString(text) |> divVector 2.0f
                sb.DrawString (sf, text, fp, Color.Lavender, 0.0f, fo, 1.0f, SpriteEffects.None, 0.5f)
                sb.End()
            | _ -> ()
            base.Draw(gameTime: GameTime)


printfn "NOTE! YOU NEED TO PUT YOUR 'MyFontFile.otf' FILE IN THE /Content DIRECTORY!!"
printfn "OTHERWISE THIS WILL NOT COMPILE"

printfn "DON'T FORGET TO RUN DOTNET RESTORE!"


// i think that the position of the text is wrong, but everything
// else seems fine

let g = new MyGame()
g.Run()
