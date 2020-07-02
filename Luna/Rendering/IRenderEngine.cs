using System.Drawing;
using Luna.Assets;
using Luna.Types;
using OpenTK.Graphics;

namespace Luna.Rendering {
    /// <summary>
    /// The render engine is meant to become the base for housing render targets,
    /// and can handle batch drawing to greatly improve performance when it's needed.
    /// 
    /// </summary>
    interface IRenderEngine {
        
    }
    /// <summary>
    /// The interface rendering targets should implement.
    /// An example of a rendering target would be the application surface,
    /// or a surface created by the game through functions.
    /// Another possibility outside the realm of being a 2.3 VM
    /// could be a networked canvas to share a surface between clients,
    /// or potentially a Luna mod that reimplements the surface for any reason,
    /// like saving or dynamically modifying it.
    /// </summary>
    interface IRenderTarget {
        #region Sprites & Tiles
        
        /// <summary>
        /// Draws a sprite with currently set colour/alpha, no rotation, and default scaling
        /// </summary>
        /// <param name="_sprite">The sprite to pick a subimage from</param>
        /// <param name="_subimage">The subimage to draw</param>
        /// <param name="_pos">Position to draw the image at </param>
        void DrawSprite(LSprite _sprite, int _subimage, PointF _pos);
        /// <summary>
        /// Draws a sprite with a different scale, rotation, colour, and alpha
        /// </summary>
        /// <param name="_sprite">The sprite to pick a subimage from</param>
        /// <param name="_subimage">The subimage to draw</param>
        /// <param name="_pos">Position to draw the image at </param>
        /// <param name="_scaling">The amount the image will be scaled by</param>
        /// <param name="_rotation">The angle at which the image will be rotated to</param>
        /// <param name="_colour">Colour and alpha of the image</param>
        void DrawSpriteExt(LSprite _sprite, int _subimage, PointF _pos, SizeF _scaling, double _rotation, LColour _colour);

        void DrawSelf(LInstance _inst);
        
        #endregion

        #region Basic Forms

        void DrawArrow();
        void DrawCircle();
        void DrawCircleColour();
        void DrawEllipse();
        void DrawEllipseColour();
        void DrawLine();
        void DrawLineColour();
        void DrawLineWidth();
        void DrawLineWidthColour();
        void DrawPoint();
        void DrawPointColour();
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
        /// <param name="_point1">Top right corner of the rectangle</param>
        /// <param name="_point2">Bottom left corner of the rectangle</param>
        /// <param name="_colour1">Top left corner's colour</param>
        /// <param name="_colour2">Top right cornet's colour</param>
        /// <param name="_colour3">Bottom right corner's colour</param>
        /// <param name="_colour4">Bottom left corner's colour</param>
        void DrawRectangleColour(PointF _point1, PointF _point2, LColour _colour1, LColour _colour2, LColour _colour3, LColour _colour4);
        void DrawRoundRectangle();
        void DrawRoundRectangleColour();
        void DrawTriangle();
        void DrawTriangleColour();
        void DrawButton();
        void DrawHealthBar();
        void DrawPath();
        
        #endregion Basic Forms
        
        #region Drawing Surfaces
        //void DrawSurface(LSurface _surface, PointF _pos);
        void DrawSurfaceExt();

        #endregion
        
        #region Text
        
        void DrawText();
        void DrawTextExt();
        void DrawTextColour();
        void DrawTextTransformed();
        void DrawTextExtColour();
        void DrawTextExtTransformed();
        void DrawTextTransformedColour();
        void DrawTextExtTransformedColour();
        void DrawHighScore();

        #endregion Text
        
        #region Primitives

        void DrawPrimitiveBegin();
        void DrawPrimitiveBeginTexture();
        void DrawPrimitiveEnd();
        void DrawVertex();
        void DrawVertexColour();
        void DrawVertexTexture();
        void DrawVertexTextureColour();

        #endregion
    }
}