using System.Drawing;
using Luna.Assets;
using Luna.Types;
using OpenTK.Graphics;

namespace Luna.Rendering {
    interface IRenderingEngine {
        #region Sprites & Tiles
        
        void DrawSprite(LSprite _sprite, int _subimage, PointF _pos);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_sprite">The sprite to pick a subimage from</param>
        /// <param name="_subimage">The subimage to draw</param>
        /// <param name="_pos">Position to draw the image at </param>
        /// <param name="_scaling"></param>
        /// <param name="_rotation"></param>
        /// <param name="_colour"></param>
        void DrawSpriteExt(LSprite _sprite, int _subimage, PointF _pos, SizeF _scaling, double _rotation, LColour _colour);
        
        #endregion

        /// <summary>
        /// Draws a rectangle using the currently set colour and alpha
        /// </summary>
        /// <param name="_point1">Top right corner of the rectangle</param>
        /// <param name="_point2">Bottom right corner of the rectangle</param>
        /// <param name="_outline">Whether to use an outline</param>
        void DrawRectangle(PointF _point1, PointF _point2, bool _outline);
        /// <summary>
        /// Draws a rectangle that have colours assigned to every corner
        /// </summary>
        /// <param name="_point1"></param>
        /// <param name="_point2"></param>
        /// <param name="_colour1"></param>
        /// <param name="_colour2"></param>
        /// <param name="_colour3"></param>
        /// <param name="_colour4"></param>
        void DrawRectangleColour(PointF _point1, PointF _point2, LColour _colour1, LColour _colour2, LColour _colour3, LColour _colour4);
    }
}