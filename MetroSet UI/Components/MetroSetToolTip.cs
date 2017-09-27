﻿using MetroSet_UI.Design;
using MetroSet_UI.Extensions;
using MetroSet_UI.Interfaces;
using MetroSet_UI.Property;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MetroSet_UI.Components
{

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(MetroSetToolTip), "Bitmaps.ToolTip.bmp")]
    [Designer(typeof(MetroSetToolTipDesigner))]
    [DefaultEvent("Popup")]
    public class MetroSetToolTip : ToolTip, iControl
    { 

        #region Interfaces

        /// <summary>
        /// Gets or sets the style associated with the control.
        /// </summary>
        [Category("MetroSet Framework"), Description("Gets or sets the style associated with the control.")]
        public Style Style
        {
            get
            {
                return StyleManager?.Style ?? style;
            }
            set
            {
                style = value;
                switch (value)
                {
                    case Style.Light:
                        ApplyTheme();
                        break;

                    case Style.Dark:
                        ApplyTheme(Style.Dark);
                        break;

                    case Style.Custom:
                        ApplyTheme(Style.Custom);
                        break;

                    default:
                        ApplyTheme();
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Style Manager associated with the control.
        /// </summary>
        [Category("MetroSet Framework"), Description("Gets or sets the Style Manager associated with the control.")]
        public StyleManager StyleManager
        {
            get { return _StyleManager; }
            set { _StyleManager = value; }
        }

        /// <summary>
        /// Gets or sets the The Author name associated with the theme.
        /// </summary>
        [Category("MetroSet Framework"), Description("Gets or sets the The Author name associated with the theme.")]
        public string ThemeAuthor { get; set; }

        /// <summary>
        /// Gets or sets the The Theme name associated with the theme.
        /// </summary>
        [Category("MetroSet Framework"), Description("Gets or sets the The Theme name associated with the theme.")]
        public string ThemeName { get; set; }

        #endregion Interfaces

        #region Global Vars

        private Methods mth;

        private Utilites utl;

        #endregion Global Vars

        #region Internal Vars

        private ToolTipProperties prop;
        private StyleManager _StyleManager;
        private Style style;

        #endregion Internal Vars

        #region Constructors

        public MetroSetToolTip()
        {
            OwnerDraw = true;
            prop = new ToolTipProperties();
            Draw += OnDraw;
            Popup += ToolTip_Popup;
            mth = new Methods();
            utl = new Utilites();
            style = Style.Light;
            ApplyTheme();
        }

        #endregion Constructors

        #region Draw Control


        private void OnDraw(object sender, DrawToolTipEventArgs e)
        {
            Graphics G = e.Graphics;
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            using (SolidBrush BG = new SolidBrush(BackColor))
            {
                using (Pen Stroke = new Pen(BorderColor))
                {
                    using (SolidBrush TB = new SolidBrush(ForeColor))
                    {
                        G.FillRectangle(BG, rect);
                        G.DrawString(e.ToolTipText, MetroSetFonts.Light(11), TB, rect, mth.SetPosition());
                        G.DrawRectangle(Stroke, rect);
                    }
                }
            }

        }

        #endregion

        #region ApplyTheme

        /// <summary>
        /// Gets or sets the style provided by the user.
        /// </summary>
        /// <param name="style">The Style.</param>
        internal void ApplyTheme(Style style = Style.Light)
        {
            switch (style)
            {
                case Style.Light:
                    prop.ForeColor = Color.FromArgb(170, 170, 170);
                    prop.BackColor = Color.White;
                    prop.BorderColor = Color.FromArgb(204, 204, 204);
                    ThemeAuthor = "Narwin";
                    ThemeName = "MetroLite";
                    SetProperties();
                    break;

                case Style.Dark:
                    prop.ForeColor = Color.FromArgb(204, 204, 204);
                    prop.BackColor = Color.FromArgb(32, 32, 32);
                    prop.BorderColor = Color.FromArgb(64, 64, 64);
                    ThemeAuthor = "Narwin";
                    ThemeName = "MetroDark";
                    SetProperties();
                    break;

                case Style.Custom:

                    if (StyleManager != null)
                        foreach (var varkey in StyleManager.ToolTipDictionary)
                        {
                            switch (varkey.Key)
                            {

                                case "BackColor":
                                    prop.BackColor = utl.HexColor((string)varkey.Value);
                                    break;

                                case "BorderColor":
                                    prop.BorderColor = utl.HexColor((string)varkey.Value);
                                    break;

                                case "ForeColor":
                                    prop.ForeColor = utl.HexColor((string)varkey.Value);
                                    break;

                                default:
                                    return;
                            }
                        }
                    SetProperties();
                    break;
            }
        }

        public void SetProperties()
        {
            try
            {
                BackColor = prop.BackColor;
                ForeColor = prop.ForeColor;
                BorderColor = prop.BorderColor;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }

        #endregion ApplyTheme

        #region Properties
        
        /// <summary>
        /// Gets or sets a value indicating whether a ToolTip window is displayed, even when its parent control is not active.
        /// </summary>
        [Browsable(false)]
        public new bool ShowAlways { get; } = false;


        /// <summary>
        /// Gets or sets a value indicating whether the ToolTip is drawn by the operating system or by code that you provide.
        /// </summary>
        [Browsable(false)]
        public new bool OwnerDraw
        {
            get { return base.OwnerDraw; }
            set { base.OwnerDraw = true; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the ToolTip should use a balloon window.
        /// </summary>
        [Browsable(false)]
        public new bool IsBalloon { get; } = false;


        /// <summary>
        /// Gets or sets the background color for the ToolTip.
        /// </summary>
        [Browsable(false)]
        public new Color BackColor { get; set; }


        /// <summary>
        /// Gets or sets the foreground color for the ToolTip.
        /// </summary>
        [Browsable(false)]
        public new Color ForeColor { get; set; }


        /// <summary>
        /// Gets or sets a title for the ToolTip window.
        /// </summary>
        [Browsable(false)]
        public new string ToolTipTitle { get; } = string.Empty;


        /// <summary>
        /// Defines a set of standardized icons that can be associated with a ToolTip.
        /// </summary>
        [Browsable(false)]
        public new ToolTipIcon ToolTipIcon { get; } = ToolTipIcon.None;


        /// <summary>
        /// Gets or sets the border color for the ToolTip.
        /// </summary>
        [Browsable(false)]
        public Color BorderColor { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The ToolTip text to display when the pointer is on the control.
        /// </summary>
        /// <param name = "control" > The Control to show the tooltip.</param>
        /// <param name = "caption" > The Text that appears in tooltip.</param>
        public new void SetToolTip(Control control, string caption)
        {
            //This Method is useful at runtime.
            base.SetToolTip(control, caption);
            foreach (Control c in control.Controls)
            {
                SetToolTip(c, caption);
            }            
        }

        #endregion

        #region Events 

        /// <summary>
        /// Here we handle popup event and we set the style of controls for tooltip.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolTip_Popup(object sender, PopupEventArgs e)
        {
            var control = e.AssociatedControl;
            if (control is iControl)
            {
                Style = ((iControl)control).Style;
                ThemeAuthor = ((iControl)control).ThemeAuthor;
                ThemeName = ((iControl)control).ThemeName;
                StyleManager = ((iControl)control).StyleManager;
            }
            else if (control is iForm)
            {
                Style = ((iForm)control).Style;
                ThemeAuthor = ((iForm)control).ThemeAuthor;
                ThemeName = ((iForm)control).ThemeName;
                StyleManager = ((iForm)control).StyleManager;
            }
            e.ToolTipSize = new Size(e.ToolTipSize.Width + 30, e.ToolTipSize.Height + 6);
        }

#endregion

    }
}