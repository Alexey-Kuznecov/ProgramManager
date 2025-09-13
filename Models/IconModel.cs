

namespace ProgramManager.Models
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using AlexLibWpf.Help;

    /// <summary>
    /// The icon model.
    /// </summary>
    [DebuggerStepThrough]
    public class IconModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IconModel"/> class.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="foreground"> The foreground. </param>
        /// <param name="background"> The background. </param>
        /// <param name="path"> The path </param>
        public IconModel(string name, string foreground, string background, string path)
        {
            this.Name = name;
            this.ForegroundColor = foreground.StringFormatToSolidColor();
            this.BackgroundColor = background.StringFormatToSolidColor();
            this.Path = new Path
            {
                Data = Geometry.Parse(path)
            };

            this.DrawIcon();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconModel"/> class.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="foreground"> The foreground. </param>
        /// <param name="background"> The background. </param>
        public IconModel(string name, SolidColorBrush foreground, SolidColorBrush background)
        {
            this.Name = name;
            this.ForegroundColor = foreground;
            this.BackgroundColor = background;
            this.DrawIcon();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconModel"/> class.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="path"> The path. </param>
        /// <param name="foreground"> The foreground. </param>
        /// <param name="background"> The background. </param>
        public IconModel(string name, Path path, SolidColorBrush foreground, SolidColorBrush background)
        {
            this.Name = name;
            this.Path = path;
            this.ForegroundColor = foreground;
            this.BackgroundColor = background;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// Gets or sets the brush.
        /// </summary>
        public DrawingBrush Brush { get; set; }

        /// <summary>
        /// Gets or sets the backgroundColor color.
        /// </summary>
        public SolidColorBrush BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foregroundColor color.
        /// </summary>
        public SolidColorBrush ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public Path Path { get; set; }

        /// <summary>
        /// Gets or sets the path list.
        /// </summary>
        public List<Path> PathList { get; set; }

        #endregion

        /// <summary>
        /// Sets an icon, uses icon resources. Sets the color of the brush.
        /// </summary>
        private void DrawIcon()
        {
            this.Brush = (DrawingBrush)Application.Current.TryFindResource(this.Name);
            DrawingBrush brush = this.Brush?.Clone();

            if (brush?.Drawing is DrawingGroup @group)
            {
                foreach (var child in group.Children)
                {
                    if (child is GeometryDrawing geometry)
                    {
                        geometry.Brush = this.ForegroundColor;
                    }
                }
            }

            this.Brush = brush;
        }
    }
}
