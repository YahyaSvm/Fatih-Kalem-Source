using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using canvas.My;

namespace canvas;

[DesignerGenerated]
public class MainWindow : Window, IComponentConnector
{
	private enum PenStyleEnum : byte
	{
		Marker,
		Stylograph,
		Highlighter
	}

	private enum DrawStateEnum : byte
	{
		Pen = 0,
		Eraser = 1,
		Line = 3,
		DashLine = 4,
		Arrow = 5,
		Rectangle = 6,
		Ellipse = 7,
		Triangle = 8,
		NoPen = 9,
		Curtain = 10
	}

	private enum RedoEnum : byte
	{
		Stroke,
		StrokeCollection,
		CurtainAdded,
		CurtainRemoved,
		CurtainRemovedAndAddedNew,
		CLS,
		LibraryItemAdded,
		LibraryCollapsed
	}

	private class isUndoneProp
	{
		public bool Stroke { get; set; }

		public bool StrokeCollection { get; set; }

		public bool CurtainAdded { get; set; }

		public bool CurtainRemoved { get; set; }

		public bool LibraryItemAdded { get; set; }

		public bool LibraryCollapsed { get; set; }

		public bool BackgroundPaperAdded { get; set; }

		public bool BackgroundCollapsed { get; set; }
	}

	private class LibraryItemProp
	{
		public int Index { get; set; }

		public double LastScaleRatio { get; set; }

		public double ActiveRotationAngle { get; set; }

		public int RemovedImageIndex { get; set; }
	}

	private enum GestureEnum : byte
	{
		Abcent,
		Left,
		Right,
		Up,
		Down
	}

	private int i;

	private int j;

	private bool IgnoreErrors;

	private double ScreenSizeRatio;

	private int LastStylusDeviceID;

	private bool DebugMode;

	private bool isFirstRun;

	private DispatcherTimer tmrDebug;

	private DispatcherTimer tmrVelocity;

	private DispatcherTimer tmrAcceleration;

	private Stopwatch swVelocity;

	private double tFirst;

	private Point pOldPos;

	private double vX;

	private double vY;

	private double tStart;

	private Color InkColor;

	private byte InkSize;

	private byte ColorNo;

	private DrawStateEnum DrawState;

	private PenStyleEnum PenStyle;

	private PenStyleEnum ActiveTab;

	private InkCanvasEditingMode CurrentEditingMode;

	private bool DragMainControl;

	private Point Drag;

	private DrawStateEnum DrawStateBeforeAction;

	private InkCanvasEditingMode EditingModeBeforeGesture;

	private bool isLeftSubMenu;

	private bool isMinimized;

	private Grid ShownSubMenu;

	private int[] FrameNoOfErase;

	private object FadeOutObject;

	private long FrameNo;

	private int[] FrameNoOfCurtainAdded;

	private int[] FrameNoOfCurtainRemoved;

	private int[] FrameNoOfStroke;

	private int iErase;

	private int iStroke;

	private int iRedo;

	private int iRemovedCurtains;

	private Rect[] RedoStackOfAddedCurtains;

	private Rect[] RedoStackOfRemovedCurtains;

	private StrokeCollection[] RedoStrokeCollectionStack;

	private Stroke[] RedoStrokeStack;

	private isUndoneProp[] Redo;

	private Rect[] UndoStackOfAddedCurtains;

	private Rect[] UndoStackOfRemovedCurtains;

	private StrokeCollection[] UndoStrokeCollectionStack;

	private bool WaitForEraserMouseUp;

	private byte IndexOfFavUndo;

	private byte IndexOfFavRedo;

	private byte iFav;

	private Image[] imgFav;

	private int iAddedCurtains;

	private string LastBrowsedPath;

	private int iLibBack;

	private Image[] imgLibBack;

	private int iCollapsedLibraries;

	private int[] FrameNoOfLibraryCollapsed;

	private int iAddedLibraryItems;

	private int[] FrameNoOfLibraryItemAdded;

	private double ActiveScaleRatio;

	private double LastScaleRatio;

	private Rect rectLibraryBorder;

	private Rect rectLibraryImage;

	private ScaleTransform transScaleOfLibrary;

	private bool DragControlOfScale;

	private double ActiveRotationAngle;

	private double LastRotationAngle;

	private RotateTransform transRotateOfLibrary;

	private bool DragControlOfRotation;

	private bool DragControlOfLibrary;

	private LibraryItemProp[] propImgLibBack;

	private int GesturesStylusDeviceID;

	private bool isGestureVisible;

	private DispatcherTimer tmrShortGesture;

	private Stopwatch swShortGesture;

	private double vShortGesture;

	private bool isGestureEnabled;

	private bool isShortGestureEnabled;

	private GestureEnum SelectedGesture;

	private Point pTouch2Start;

	private int iCollapsedBackgrounds;

	private int[] FrameNoOfBackgroundCollapsed;

	private int iAddedBackgroundPapers;

	private int[] FrameNoOfBackgroundPaperAdded;

	private DateTime tLastClick;

	private DispatcherTimer tmrReShowSideArrowLeft;

	private DispatcherTimer tmrReShowSideArrowRight;

	private DispatcherTimer tmrSideArrows;

	private Stopwatch swQuickAccess;

	private double vQuickAccess;

	private bool isSideArrowsEnabled;

	private byte ShapeSize;

	private bool isDraw;

	private bool isTrianglesSecondStep;

	private Point[] TrianglePoints;

	private Point pStart;

	private StylusPointCollection pts;

	private Stroke st;

	private Stroke st2;

	private Stroke[] strokeDash;

	[CompilerGenerated]
	[AccessedThroughProperty("inkCanvas")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private InkCanvas _inkCanvas;

	[CompilerGenerated]
	[AccessedThroughProperty("rectLibrary")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Rectangle _rectLibrary;

	[CompilerGenerated]
	[AccessedThroughProperty("imgResize")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgResize;

	[CompilerGenerated]
	[AccessedThroughProperty("imgRotate")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgRotate;

	[CompilerGenerated]
	[AccessedThroughProperty("imgTitleBar")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgTitleBar;

	[CompilerGenerated]
	[AccessedThroughProperty("imgHandAndPen")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgHandAndPen;

	[CompilerGenerated]
	[AccessedThroughProperty("imgPen")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgPen;

	[CompilerGenerated]
	[AccessedThroughProperty("imgEraser")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgEraser;

	[CompilerGenerated]
	[AccessedThroughProperty("imgShape")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgShape;

	[CompilerGenerated]
	[AccessedThroughProperty("imgClose")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgClose;

	[CompilerGenerated]
	[AccessedThroughProperty("imgSettings")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgSettings;

	[CompilerGenerated]
	[AccessedThroughProperty("GridSubMenuPenRight")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Grid _GridSubMenuPenRight;

	[CompilerGenerated]
	[AccessedThroughProperty("GridSubMenuPenLeft")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Grid _GridSubMenuPenLeft;

	[CompilerGenerated]
	[AccessedThroughProperty("GridSubMenuEraserRight")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Grid _GridSubMenuEraserRight;

	[CompilerGenerated]
	[AccessedThroughProperty("GridSubMenuEraserLeft")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Grid _GridSubMenuEraserLeft;

	[CompilerGenerated]
	[AccessedThroughProperty("GridSubMenuShapeRight")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Grid _GridSubMenuShapeRight;

	[CompilerGenerated]
	[AccessedThroughProperty("GridSubMenuShapeLeft")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Grid _GridSubMenuShapeLeft;

	[CompilerGenerated]
	[AccessedThroughProperty("lblCloseRight")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblCloseRight;

	[CompilerGenerated]
	[AccessedThroughProperty("lblCloseLeft")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblCloseLeft;

	[CompilerGenerated]
	[AccessedThroughProperty("imgColorSelector")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgColorSelector;

	[CompilerGenerated]
	[AccessedThroughProperty("imgCloseSettings")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgCloseSettings;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSettings")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSettings;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSettingsStartingPen")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSettingsStartingPen;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSettingsStartingPosition")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSettingsStartingPosition;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSettingsFavourites")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSettingsFavourites;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSettingsSpeedAccess")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSettingsSpeedAccess;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSettingsCloseConfirmation")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSettingsCloseConfirmation;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSettingsAutoStart")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSettingsAutoStart;

	[CompilerGenerated]
	[AccessedThroughProperty("lblAbout")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblAbout;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSetCurrentPenState")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSetCurrentPenState;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSetDefaultPen")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSetDefaultPen;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSetCurrentPosition")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSetCurrentPosition;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSetDefaultPosition")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSetDefaultPosition;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSaveFavourites")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSaveFavourites;

	[CompilerGenerated]
	[AccessedThroughProperty("lblSetDefaultFavourites")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblSetDefaultFavourites;

	[CompilerGenerated]
	[AccessedThroughProperty("chkGesture")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private CheckBox _chkGesture;

	[CompilerGenerated]
	[AccessedThroughProperty("chkShortGesture")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private CheckBox _chkShortGesture;

	[CompilerGenerated]
	[AccessedThroughProperty("chkSideArrows")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private CheckBox _chkSideArrows;

	[CompilerGenerated]
	[AccessedThroughProperty("chkCloseConfirmation")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private CheckBox _chkCloseConfirmation;

	[CompilerGenerated]
	[AccessedThroughProperty("chkAutoStart")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private CheckBox _chkAutoStart;

	[CompilerGenerated]
	[AccessedThroughProperty("chkPreLoad")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private CheckBox _chkPreLoad;

	[CompilerGenerated]
	[AccessedThroughProperty("lblWebLink")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Label _lblWebLink;

	[CompilerGenerated]
	[AccessedThroughProperty("imgSideArrowRight")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgSideArrowRight;

	[CompilerGenerated]
	[AccessedThroughProperty("imgSideArrowLeft")]
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	private Image _imgSideArrowLeft;

	private bool _contentLoaded;

	[field: AccessedThroughProperty("borderImages")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Border borderImages
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgHandRes")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgHandRes
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgPenDrawing")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgPenDrawing
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgPenRed")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgPenRed
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgPenBlue")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgPenBlue
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgPenGreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgPenGreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgPenOrange")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgPenOrange
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgPenBlack")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgPenBlack
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgPenWhite")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgPenWhite
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgPenColorful")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgPenColorful
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgNoPen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgNoPen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgStylographRed")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgStylographRed
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgStylographBlue")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgStylographBlue
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgStylographGreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgStylographGreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgStylographOrange")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgStylographOrange
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgStylographBlack")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgStylographBlack
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgStylographWhite")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgStylographWhite
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgStylographColorful")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgStylographColorful
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgStylographNoPen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgStylographNoPen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgHighlighterRed")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgHighlighterRed
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgHighlighterBlue")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgHighlighterBlue
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgHighlighterGreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgHighlighterGreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgHighlighterOrange")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgHighlighterOrange
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgHighlighterBlack")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgHighlighterBlack
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgHighlighterWhite")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgHighlighterWhite
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgHighlighterColorful")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgHighlighterColorful
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgHighlighterNoPen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgHighlighterNoPen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSubMenuMarkerRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuMarkerRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSubMenuMarkerLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuMarkerLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSubMenuStylographRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuStylographRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSubMenuStylographLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuStylographLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSubMenuHighlighterRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuHighlighterRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSubMenuHighlighterLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuHighlighterLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureEraser")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureEraser
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureMarkerRed")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureMarkerRed
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureMarkerBlue")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureMarkerBlue
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureMarkerGreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureMarkerGreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureMarkerBlack")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureMarkerBlack
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureStylographRed")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureStylographRed
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureStylographBlue")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureStylographBlue
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureStylographGreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureStylographGreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureStylographBlack")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureStylographBlack
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureHighlighterRed")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureHighlighterRed
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureHighlighterBlue")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureHighlighterBlue
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureHighlighterGreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureHighlighterGreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureHighlighterYellow")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureHighlighterYellow
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavArrow")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavArrow
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavBackground")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavBackground
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavCurtain")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavCurtain
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavDashLine")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavDashLine
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavEllipse")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavEllipse
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavEraser1")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavEraser1
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavEraser2")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavEraser2
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavEraser3")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavEraser3
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavEraser4")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavEraser4
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavGrayedRedo")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavGrayedRedo
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavGrayedUndo")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavGrayedUndo
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavHighlighterBlack")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavHighlighterBlack
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavHighlighterBlue")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavHighlighterBlue
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavHighlighterColorful")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavHighlighterColorful
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavHighlighterGreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavHighlighterGreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavHighlighterNoPen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavHighlighterNoPen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavHighlighterRed")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavHighlighterRed
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavHighlighterWhite")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavHighlighterWhite
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavHighlighterYellow")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavHighlighterYellow
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavLibrary")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavLibrary
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavLine")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavLine
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavMarkerBlack")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavMarkerBlack
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavMarkerBlue")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavMarkerBlue
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavMarkerColorFul")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavMarkerColorFul
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavMarkerGreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavMarkerGreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavMarkerNoPen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavMarkerNoPen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavMarkerOrange")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavMarkerOrange
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavMarkerRed")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavMarkerRed
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavMarkerWhite")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavMarkerWhite
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavRectangle")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavRectangle
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavRedo")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavRedo
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavSize1")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavSize1
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavSize2")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavSize2
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavSize3")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavSize3
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavSize4")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavSize4
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavSize5")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavSize5
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavSize6")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavSize6
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavStylographBlack")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavStylographBlack
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavStylographBlue")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavStylographBlue
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavStylographColorful")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavStylographColorful
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavStylographGreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavStylographGreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavStylographNoPen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavStylographNoPen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavStylographOrange")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavStylographOrange
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavStylographRed")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavStylographRed
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavStylographWhite")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavStylographWhite
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavTriangle")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavTriangle
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFavUndo")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFavUndo
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("cnvBackgroundPaper")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Canvas cnvBackgroundPaper
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("cnvCurtain")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Canvas cnvCurtain
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("pathCurtain")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Path pathCurtain
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("rectOpaque")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual RectangleGeometry rectOpaque
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("rectTransparent")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual RectangleGeometry rectTransparent
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("cnvLibraryBack")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Canvas cnvLibraryBack
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("gridGesture")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid gridGesture
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("cnvGesture")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Canvas cnvGesture
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("gridExpandingCircle")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid gridExpandingCircle
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("pathExpandingCircleDown")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Path pathExpandingCircleDown
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("ellipseExpandingDown")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual EllipseGeometry ellipseExpandingDown
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("pathExpandingCircleRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Path pathExpandingCircleRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("ellipseExpandingRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual EllipseGeometry ellipseExpandingRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("pathExpandingCircleUp")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Path pathExpandingCircleUp
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("ellipseExpandingUp")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual EllipseGeometry ellipseExpandingUp
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("pathExpandingCircleLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Path pathExpandingCircleLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("ellipseExpandingLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual EllipseGeometry ellipseExpandingLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureUp")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureUp
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureDown")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureDown
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGestureAnimation")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGestureAnimation
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual InkCanvas inkCanvas
	{
		[CompilerGenerated]
		get
		{
			return _inkCanvas;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = inkCanvas_MouseLeftButtonUp;
			MouseEventHandler value3 = inkCanvas_MouseMove;
			MouseButtonEventHandler value4 = inkCanvas_PreviewMouseLeftButtonDown;
			MouseButtonEventHandler value5 = inkCanvas_PreviewMouseRightButtonDown;
			MouseButtonEventHandler value6 = inkCanvas_PreviewMouseRightButtonUp;
			InkCanvasStrokeCollectedEventHandler value7 = inkCanvas_StrokeCollected;
			InkCanvasStrokeErasingEventHandler value8 = inkCanvas_StrokeErasing;
			StylusDownEventHandler value9 = inkCanvas_PreviewStylusDown;
			StylusEventHandler value10 = inkCanvas_PreviewStylusMove;
			StylusEventHandler value11 = inkCanvas_PreviewStylusUp;
			InkCanvas inkCanvas = _inkCanvas;
			if (inkCanvas != null)
			{
				inkCanvas.MouseLeftButtonUp -= value2;
				inkCanvas.MouseMove -= value3;
				inkCanvas.PreviewMouseLeftButtonDown -= value4;
				inkCanvas.PreviewMouseRightButtonDown -= value5;
				inkCanvas.PreviewMouseRightButtonUp -= value6;
				inkCanvas.StrokeCollected -= value7;
				inkCanvas.StrokeErasing -= value8;
				inkCanvas.PreviewStylusDown -= value9;
				inkCanvas.PreviewStylusMove -= value10;
				inkCanvas.PreviewStylusUp -= value11;
			}
			_inkCanvas = value;
			inkCanvas = _inkCanvas;
			if (inkCanvas != null)
			{
				inkCanvas.MouseLeftButtonUp += value2;
				inkCanvas.MouseMove += value3;
				inkCanvas.PreviewMouseLeftButtonDown += value4;
				inkCanvas.PreviewMouseRightButtonDown += value5;
				inkCanvas.PreviewMouseRightButtonUp += value6;
				inkCanvas.StrokeCollected += value7;
				inkCanvas.StrokeErasing += value8;
				inkCanvas.PreviewStylusDown += value9;
				inkCanvas.PreviewStylusMove += value10;
				inkCanvas.PreviewStylusUp += value11;
			}
		}
	}

	[field: AccessedThroughProperty("cnvLibraryFront")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Canvas cnvLibraryFront
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("cnvLibrary")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Canvas cnvLibrary
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgLibrary")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgLibrary
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Rectangle rectLibrary
	{
		[CompilerGenerated]
		get
		{
			return _rectLibrary;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = rectLibrary_MouseDown;
			MouseEventHandler value3 = rectLibrary_MouseMove;
			MouseButtonEventHandler value4 = rectLibrary_MouseUp;
			Rectangle rectangle = _rectLibrary;
			if (rectangle != null)
			{
				rectangle.MouseDown -= value2;
				rectangle.MouseMove -= value3;
				rectangle.MouseUp -= value4;
			}
			_rectLibrary = value;
			rectangle = _rectLibrary;
			if (rectangle != null)
			{
				rectangle.MouseDown += value2;
				rectangle.MouseMove += value3;
				rectangle.MouseUp += value4;
			}
		}
	}

	internal virtual Image imgResize
	{
		[CompilerGenerated]
		get
		{
			return _imgResize;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgResize_MouseDown;
			MouseEventHandler value3 = imgResize_MouseMove;
			MouseButtonEventHandler value4 = imgResize_MouseUp;
			Image image = _imgResize;
			if (image != null)
			{
				image.MouseDown -= value2;
				image.MouseMove -= value3;
				image.MouseUp -= value4;
			}
			_imgResize = value;
			image = _imgResize;
			if (image != null)
			{
				image.MouseDown += value2;
				image.MouseMove += value3;
				image.MouseUp += value4;
			}
		}
	}

	internal virtual Image imgRotate
	{
		[CompilerGenerated]
		get
		{
			return _imgRotate;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgRotate_MouseDown;
			MouseEventHandler value3 = imgRotate_MouseMove;
			MouseButtonEventHandler value4 = imgRotate_MouseUp;
			Image image = _imgRotate;
			if (image != null)
			{
				image.MouseDown -= value2;
				image.MouseMove -= value3;
				image.MouseUp -= value4;
			}
			_imgRotate = value;
			image = _imgRotate;
			if (image != null)
			{
				image.MouseDown += value2;
				image.MouseMove += value3;
				image.MouseUp += value4;
			}
		}
	}

	[field: AccessedThroughProperty("rectFrame")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Rectangle rectFrame
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("cnvMainMenu")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Canvas cnvMainMenu
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("borderMainMenu")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Border borderMainMenu
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("gridMainMenu")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid gridMainMenu
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Image imgTitleBar
	{
		[CompilerGenerated]
		get
		{
			return _imgTitleBar;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgTitleBar_PreviewMouseLeftButtonDown;
			MouseEventHandler value3 = imgTitleBar_PreviewMouseMove;
			MouseButtonEventHandler value4 = imgTitleBar_PreviewMouseLeftButtonUp;
			Image image = _imgTitleBar;
			if (image != null)
			{
				image.PreviewMouseLeftButtonDown -= value2;
				image.PreviewMouseMove -= value3;
				image.PreviewMouseLeftButtonUp -= value4;
			}
			_imgTitleBar = value;
			image = _imgTitleBar;
			if (image != null)
			{
				image.PreviewMouseLeftButtonDown += value2;
				image.PreviewMouseMove += value3;
				image.PreviewMouseLeftButtonUp += value4;
			}
		}
	}

	internal virtual Image imgHandAndPen
	{
		[CompilerGenerated]
		get
		{
			return _imgHandAndPen;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgHandAndPen_PreviewMouseLeftButtonUp;
			Image image = _imgHandAndPen;
			if (image != null)
			{
				image.PreviewMouseLeftButtonUp -= value2;
			}
			_imgHandAndPen = value;
			image = _imgHandAndPen;
			if (image != null)
			{
				image.PreviewMouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Image imgPen
	{
		[CompilerGenerated]
		get
		{
			return _imgPen;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgPen_MouseLeftButtonUp;
			Image image = _imgPen;
			if (image != null)
			{
				image.MouseLeftButtonUp -= value2;
			}
			_imgPen = value;
			image = _imgPen;
			if (image != null)
			{
				image.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("imgTickOfPen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgTickOfPen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Image imgEraser
	{
		[CompilerGenerated]
		get
		{
			return _imgEraser;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgEraser_MouseLeftButtonUp;
			Image image = _imgEraser;
			if (image != null)
			{
				image.MouseLeftButtonUp -= value2;
			}
			_imgEraser = value;
			image = _imgEraser;
			if (image != null)
			{
				image.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("imgTickOfEraser")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgTickOfEraser
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Image imgShape
	{
		[CompilerGenerated]
		get
		{
			return _imgShape;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgShape_MouseLeftButtonUp;
			Image image = _imgShape;
			if (image != null)
			{
				image.MouseLeftButtonUp -= value2;
			}
			_imgShape = value;
			image = _imgShape;
			if (image != null)
			{
				image.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("imgTickOfShape")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgTickOfShape
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Image imgClose
	{
		[CompilerGenerated]
		get
		{
			return _imgClose;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = close_MouseLeftButtonUp;
			Image image = _imgClose;
			if (image != null)
			{
				image.MouseLeftButtonUp -= value2;
			}
			_imgClose = value;
			image = _imgClose;
			if (image != null)
			{
				image.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Image imgSettings
	{
		[CompilerGenerated]
		get
		{
			return _imgSettings;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = Settings_MouseLeftButtonUp;
			Image image = _imgSettings;
			if (image != null)
			{
				image.MouseLeftButtonUp -= value2;
			}
			_imgSettings = value;
			image = _imgSettings;
			if (image != null)
			{
				image.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Grid GridSubMenuPenRight
	{
		[CompilerGenerated]
		get
		{
			return _GridSubMenuPenRight;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = [SpecialName] (object a0, MouseButtonEventArgs a1) =>
			{
				SubMenuPen_MouseLeftButtonUp(RuntimeHelpers.GetObjectValue(a0), a1);
			};
			MouseButtonEventHandler value3 = SubMenuPen_MouseRightButtonUp;
			Grid grid = _GridSubMenuPenRight;
			if (grid != null)
			{
				grid.MouseLeftButtonUp -= value2;
				grid.MouseRightButtonUp -= value3;
			}
			_GridSubMenuPenRight = value;
			grid = _GridSubMenuPenRight;
			if (grid != null)
			{
				grid.MouseLeftButtonUp += value2;
				grid.MouseRightButtonUp += value3;
			}
		}
	}

	[field: AccessedThroughProperty("imgSubMenuPenRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuPenRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize1SelectionR")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize1SelectionR
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize2SelectionR")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize2SelectionR
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize3SelectionR")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize3SelectionR
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize4SelectionR")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize4SelectionR
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize5SelectionR")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize5SelectionR
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize6SelectionR")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize6SelectionR
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Grid GridSubMenuPenLeft
	{
		[CompilerGenerated]
		get
		{
			return _GridSubMenuPenLeft;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = [SpecialName] (object a0, MouseButtonEventArgs a1) =>
			{
				SubMenuPen_MouseLeftButtonUp(RuntimeHelpers.GetObjectValue(a0), a1);
			};
			MouseButtonEventHandler value3 = SubMenuPen_MouseRightButtonUp;
			Grid grid = _GridSubMenuPenLeft;
			if (grid != null)
			{
				grid.MouseLeftButtonUp -= value2;
				grid.MouseRightButtonUp -= value3;
			}
			_GridSubMenuPenLeft = value;
			grid = _GridSubMenuPenLeft;
			if (grid != null)
			{
				grid.MouseLeftButtonUp += value2;
				grid.MouseRightButtonUp += value3;
			}
		}
	}

	[field: AccessedThroughProperty("imgSubMenuPenLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuPenLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize1SelectionL")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize1SelectionL
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize2SelectionL")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize2SelectionL
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize3SelectionL")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize3SelectionL
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize4SelectionL")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize4SelectionL
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize5SelectionL")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize5SelectionL
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSize6SelectionL")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSize6SelectionL
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Grid GridSubMenuEraserRight
	{
		[CompilerGenerated]
		get
		{
			return _GridSubMenuEraserRight;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = [SpecialName] (object a0, MouseButtonEventArgs a1) =>
			{
				SubMenuEraser_MouseLeftButtonUp(RuntimeHelpers.GetObjectValue(a0), a1);
			};
			MouseButtonEventHandler value3 = SubMenuEraser_MouseRightButtonUp;
			Grid grid = _GridSubMenuEraserRight;
			if (grid != null)
			{
				grid.MouseLeftButtonUp -= value2;
				grid.MouseRightButtonUp -= value3;
			}
			_GridSubMenuEraserRight = value;
			grid = _GridSubMenuEraserRight;
			if (grid != null)
			{
				grid.MouseLeftButtonUp += value2;
				grid.MouseRightButtonUp += value3;
			}
		}
	}

	[field: AccessedThroughProperty("imgSubMenuEraserRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuEraserRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGrayedUndoRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGrayedUndoRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGrayedRedoRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGrayedRedoRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Grid GridSubMenuEraserLeft
	{
		[CompilerGenerated]
		get
		{
			return _GridSubMenuEraserLeft;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = [SpecialName] (object a0, MouseButtonEventArgs a1) =>
			{
				SubMenuEraser_MouseLeftButtonUp(RuntimeHelpers.GetObjectValue(a0), a1);
			};
			MouseButtonEventHandler value3 = SubMenuEraser_MouseRightButtonUp;
			Grid grid = _GridSubMenuEraserLeft;
			if (grid != null)
			{
				grid.MouseLeftButtonUp -= value2;
				grid.MouseRightButtonUp -= value3;
			}
			_GridSubMenuEraserLeft = value;
			grid = _GridSubMenuEraserLeft;
			if (grid != null)
			{
				grid.MouseLeftButtonUp += value2;
				grid.MouseRightButtonUp += value3;
			}
		}
	}

	[field: AccessedThroughProperty("imgSubMenuEraserLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuEraserLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGrayedUndoLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGrayedUndoLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgGrayedRedoLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgGrayedRedoLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Grid GridSubMenuShapeRight
	{
		[CompilerGenerated]
		get
		{
			return _GridSubMenuShapeRight;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = [SpecialName] (object a0, MouseButtonEventArgs a1) =>
			{
				SubMenuShape_MouseLeftButtonUp(RuntimeHelpers.GetObjectValue(a0), a1);
			};
			MouseButtonEventHandler value3 = SubMenuShape_MouseRightButtonUp;
			Grid grid = _GridSubMenuShapeRight;
			if (grid != null)
			{
				grid.MouseLeftButtonUp -= value2;
				grid.MouseRightButtonUp -= value3;
			}
			_GridSubMenuShapeRight = value;
			grid = _GridSubMenuShapeRight;
			if (grid != null)
			{
				grid.MouseLeftButtonUp += value2;
				grid.MouseRightButtonUp += value3;
			}
		}
	}

	[field: AccessedThroughProperty("imgSubMenuShapeRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuShapeRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Grid GridSubMenuShapeLeft
	{
		[CompilerGenerated]
		get
		{
			return _GridSubMenuShapeLeft;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = [SpecialName] (object a0, MouseButtonEventArgs a1) =>
			{
				SubMenuShape_MouseLeftButtonUp(RuntimeHelpers.GetObjectValue(a0), a1);
			};
			MouseButtonEventHandler value3 = SubMenuShape_MouseRightButtonUp;
			Grid grid = _GridSubMenuShapeLeft;
			if (grid != null)
			{
				grid.MouseLeftButtonUp -= value2;
				grid.MouseRightButtonUp -= value3;
			}
			_GridSubMenuShapeLeft = value;
			grid = _GridSubMenuShapeLeft;
			if (grid != null)
			{
				grid.MouseLeftButtonUp += value2;
				grid.MouseRightButtonUp += value3;
			}
		}
	}

	[field: AccessedThroughProperty("imgSubMenuShapeLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuShapeLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("GridSubMenuCloseRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridSubMenuCloseRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSubMenuCloseRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuCloseRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Label lblCloseRight
	{
		[CompilerGenerated]
		get
		{
			return _lblCloseRight;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = CloseMe;
			Label label = _lblCloseRight;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblCloseRight = value;
			label = _lblCloseRight;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("GridSubMenuCloseLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridSubMenuCloseLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgSubMenuCloseLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgSubMenuCloseLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Label lblCloseLeft
	{
		[CompilerGenerated]
		get
		{
			return _lblCloseLeft;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = CloseMe;
			Label label = _lblCloseLeft;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblCloseLeft = value;
			label = _lblCloseLeft;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("GridColorSelector")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridColorSelector
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Image imgColorSelector
	{
		[CompilerGenerated]
		get
		{
			return _imgColorSelector;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgColorSelector_MouseLeftButtonUp;
			Image image = _imgColorSelector;
			if (image != null)
			{
				image.MouseLeftButtonUp -= value2;
			}
			_imgColorSelector = value;
			image = _imgColorSelector;
			if (image != null)
			{
				image.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("borderSettings")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Border borderSettings
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Image imgCloseSettings
	{
		[CompilerGenerated]
		get
		{
			return _imgCloseSettings;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgCloseSettings_MouseLeftButtonUp;
			Image image = _imgCloseSettings;
			if (image != null)
			{
				image.MouseLeftButtonUp -= value2;
			}
			_imgCloseSettings = value;
			image = _imgCloseSettings;
			if (image != null)
			{
				image.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSettings
	{
		[CompilerGenerated]
		get
		{
			return _lblSettings;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSettingsMenuSettings_MouseLeftButtonUp;
			Label label = _lblSettings;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSettings = value;
			label = _lblSettings;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSettingsStartingPen
	{
		[CompilerGenerated]
		get
		{
			return _lblSettingsStartingPen;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSettingsMenuStartingPen_MouseLeftButtonUp;
			Label label = _lblSettingsStartingPen;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSettingsStartingPen = value;
			label = _lblSettingsStartingPen;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSettingsStartingPosition
	{
		[CompilerGenerated]
		get
		{
			return _lblSettingsStartingPosition;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSettingsMenuStartingPosition_MouseLeftButtonUp;
			Label label = _lblSettingsStartingPosition;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSettingsStartingPosition = value;
			label = _lblSettingsStartingPosition;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSettingsFavourites
	{
		[CompilerGenerated]
		get
		{
			return _lblSettingsFavourites;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSettingsFavourites_MouseLeftButtonUp;
			Label label = _lblSettingsFavourites;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSettingsFavourites = value;
			label = _lblSettingsFavourites;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSettingsSpeedAccess
	{
		[CompilerGenerated]
		get
		{
			return _lblSettingsSpeedAccess;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSettingsMenuSpeedAccess_MouseLeftButtonUp;
			Label label = _lblSettingsSpeedAccess;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSettingsSpeedAccess = value;
			label = _lblSettingsSpeedAccess;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSettingsCloseConfirmation
	{
		[CompilerGenerated]
		get
		{
			return _lblSettingsCloseConfirmation;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSettingsCloseConfirmation_MouseLeftButtonUp;
			Label label = _lblSettingsCloseConfirmation;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSettingsCloseConfirmation = value;
			label = _lblSettingsCloseConfirmation;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSettingsAutoStart
	{
		[CompilerGenerated]
		get
		{
			return _lblSettingsAutoStart;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSettingsMenuAutoStart_MouseLeftButtonUp;
			Label label = _lblSettingsAutoStart;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSettingsAutoStart = value;
			label = _lblSettingsAutoStart;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblAbout
	{
		[CompilerGenerated]
		get
		{
			return _lblAbout;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblAboutMenu_MouseLeftButtonUp;
			Label label = _lblAbout;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblAbout = value;
			label = _lblAbout;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("GridLogo")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridLogo
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgFatihPen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgFatihPen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("lblVersion")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Label lblVersion
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("GridStartingPen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridStartingPen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("lblPenType")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Label lblPenType
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("lblInkSize")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Label lblInkSize
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("lblInkColor")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Label lblInkColor
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("ellipseInkColor")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Ellipse ellipseInkColor
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Label lblSetCurrentPenState
	{
		[CompilerGenerated]
		get
		{
			return _lblSetCurrentPenState;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSetCurrentPenState_MouseLeftButtonUp;
			Label label = _lblSetCurrentPenState;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSetCurrentPenState = value;
			label = _lblSetCurrentPenState;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSetDefaultPen
	{
		[CompilerGenerated]
		get
		{
			return _lblSetDefaultPen;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSetDefaultPen_MouseLeftButtonUp;
			Label label = _lblSetDefaultPen;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSetDefaultPen = value;
			label = _lblSetDefaultPen;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("GridStartingPosition")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridStartingPosition
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("rectScreen")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Rectangle rectScreen
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgMiniMainMenu")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgMiniMainMenu
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Label lblSetCurrentPosition
	{
		[CompilerGenerated]
		get
		{
			return _lblSetCurrentPosition;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSetCurrentPosition_MouseLeftButtonUp;
			Label label = _lblSetCurrentPosition;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSetCurrentPosition = value;
			label = _lblSetCurrentPosition;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSetDefaultPosition
	{
		[CompilerGenerated]
		get
		{
			return _lblSetDefaultPosition;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSetDefaultPosition_MouseLeftButtonUp;
			Label label = _lblSetDefaultPosition;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSetDefaultPosition = value;
			label = _lblSetDefaultPosition;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("GridFavourites")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridFavourites
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("textBlockFovourites")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual TextBlock textBlockFovourites
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Label lblSaveFavourites
	{
		[CompilerGenerated]
		get
		{
			return _lblSaveFavourites;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSaveFavourites_MouseLeftButtonUp;
			Label label = _lblSaveFavourites;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSaveFavourites = value;
			label = _lblSaveFavourites;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	internal virtual Label lblSetDefaultFavourites
	{
		[CompilerGenerated]
		get
		{
			return _lblSetDefaultFavourites;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblSetDefaultFavourites_MouseLeftButtonUp;
			Label label = _lblSetDefaultFavourites;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblSetDefaultFavourites = value;
			label = _lblSetDefaultFavourites;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("GridSpeedAccess")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridSpeedAccess
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("rectScreen2")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Rectangle rectScreen2
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgsettingsGestureEllipse")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgsettingsGestureEllipse
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("imgsettingsGestureHand2Finger")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Image imgsettingsGestureHand2Finger
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual CheckBox chkGesture
	{
		[CompilerGenerated]
		get
		{
			return _chkGesture;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			RoutedEventHandler value2 = chkGestures_Click;
			CheckBox checkBox = _chkGesture;
			if (checkBox != null)
			{
				checkBox.Click -= value2;
			}
			_chkGesture = value;
			checkBox = _chkGesture;
			if (checkBox != null)
			{
				checkBox.Click += value2;
			}
		}
	}

	internal virtual CheckBox chkShortGesture
	{
		[CompilerGenerated]
		get
		{
			return _chkShortGesture;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			RoutedEventHandler value2 = chkShortGesture_Click;
			CheckBox checkBox = _chkShortGesture;
			if (checkBox != null)
			{
				checkBox.Click -= value2;
			}
			_chkShortGesture = value;
			checkBox = _chkShortGesture;
			if (checkBox != null)
			{
				checkBox.Click += value2;
			}
		}
	}

	internal virtual CheckBox chkSideArrows
	{
		[CompilerGenerated]
		get
		{
			return _chkSideArrows;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			RoutedEventHandler value2 = chkSideArrows_Click;
			CheckBox checkBox = _chkSideArrows;
			if (checkBox != null)
			{
				checkBox.Click -= value2;
			}
			_chkSideArrows = value;
			checkBox = _chkSideArrows;
			if (checkBox != null)
			{
				checkBox.Click += value2;
			}
		}
	}

	[field: AccessedThroughProperty("GridCloseConfirmation")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridCloseConfirmation
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual CheckBox chkCloseConfirmation
	{
		[CompilerGenerated]
		get
		{
			return _chkCloseConfirmation;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			RoutedEventHandler value2 = chkCloseConfirmation_Click;
			CheckBox checkBox = _chkCloseConfirmation;
			if (checkBox != null)
			{
				checkBox.Click -= value2;
			}
			_chkCloseConfirmation = value;
			checkBox = _chkCloseConfirmation;
			if (checkBox != null)
			{
				checkBox.Click += value2;
			}
		}
	}

	[field: AccessedThroughProperty("GridAutoStart")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridAutoStart
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual CheckBox chkAutoStart
	{
		[CompilerGenerated]
		get
		{
			return _chkAutoStart;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			RoutedEventHandler value2 = chkAutoStart_Click;
			CheckBox checkBox = _chkAutoStart;
			if (checkBox != null)
			{
				checkBox.Click -= value2;
			}
			_chkAutoStart = value;
			checkBox = _chkAutoStart;
			if (checkBox != null)
			{
				checkBox.Click += value2;
			}
		}
	}

	internal virtual CheckBox chkPreLoad
	{
		[CompilerGenerated]
		get
		{
			return _chkPreLoad;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			RoutedEventHandler value2 = chkPreLoad_Click;
			CheckBox checkBox = _chkPreLoad;
			if (checkBox != null)
			{
				checkBox.Click -= value2;
			}
			_chkPreLoad = value;
			checkBox = _chkPreLoad;
			if (checkBox != null)
			{
				checkBox.Click += value2;
			}
		}
	}

	[field: AccessedThroughProperty("textChkPreLoad")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual AccessText textChkPreLoad
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("GridAbout")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Grid GridAbout
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("textBlockAbout")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual TextBlock textBlockAbout
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Label lblWebLink
	{
		[CompilerGenerated]
		get
		{
			return _lblWebLink;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = lblWebLink_MouseLeftButtonUp;
			Label label = _lblWebLink;
			if (label != null)
			{
				label.MouseLeftButtonUp -= value2;
			}
			_lblWebLink = value;
			label = _lblWebLink;
			if (label != null)
			{
				label.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("borderSideArrowRight")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Border borderSideArrowRight
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Image imgSideArrowRight
	{
		[CompilerGenerated]
		get
		{
			return _imgSideArrowRight;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgSideArrowRight_MouseLeftButtonUp;
			Image image = _imgSideArrowRight;
			if (image != null)
			{
				image.MouseLeftButtonUp -= value2;
			}
			_imgSideArrowRight = value;
			image = _imgSideArrowRight;
			if (image != null)
			{
				image.MouseLeftButtonUp += value2;
			}
		}
	}

	[field: AccessedThroughProperty("borderSideArrowLeft")]
	[field: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	internal virtual Border borderSideArrowLeft
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	internal virtual Image imgSideArrowLeft
	{
		[CompilerGenerated]
		get
		{
			return _imgSideArrowLeft;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			MouseButtonEventHandler value2 = imgSideArrowLeft_MouseLeftButtonUp;
			Image image = _imgSideArrowLeft;
			if (image != null)
			{
				image.MouseLeftButtonUp -= value2;
			}
			_imgSideArrowLeft = value;
			image = _imgSideArrowLeft;
			if (image != null)
			{
				image.MouseLeftButtonUp += value2;
			}
		}
	}

	public MainWindow()
	{
		base.Loaded += Window_Loaded;
		base.Activated += MainWindow_Activated;
		base.Deactivated += MainWindow_Deactivated;
		base.Deactivated += MyBase_Deactivated;
		base.StateChanged += MainWindow_StateChanged;
		base.PreviewStylusUp += MainWindow_PreviewStylusUp;
		base.StylusEnter += MainWindow_StylusEnter;
		base.StylusLeave += MainWindow_StylusLeave;
		base.PreviewMouseRightButtonUp += MainWindow_PreviewMouseRightButtonUp;
		base.MouseEnter += MainWindow_MouseEnter;
		base.MouseLeave += MainWindow_MouseLeave;
		ScreenSizeRatio = SystemParameters.FullPrimaryScreenWidth / 1920.0;
		LastStylusDeviceID = -2;
		DebugMode = Debugger.IsAttached;
		isFirstRun = true;
		swVelocity = new Stopwatch();
		FrameNoOfErase = new int[1];
		FrameNoOfCurtainAdded = new int[1];
		FrameNoOfCurtainRemoved = new int[1];
		FrameNoOfStroke = new int[1];
		RedoStackOfAddedCurtains = new Rect[1];
		RedoStackOfRemovedCurtains = new Rect[1];
		RedoStrokeCollectionStack = new StrokeCollection[1];
		RedoStrokeStack = new Stroke[1];
		Redo = new isUndoneProp[1];
		UndoStackOfAddedCurtains = new Rect[1];
		UndoStackOfRemovedCurtains = new Rect[1];
		UndoStrokeCollectionStack = new StrokeCollection[1];
		imgFav = new Image[21];
		imgLibBack = new Image[1];
		FrameNoOfLibraryCollapsed = new int[1];
		FrameNoOfLibraryItemAdded = new int[1];
		propImgLibBack = new LibraryItemProp[1];
		GesturesStylusDeviceID = -2;
		swShortGesture = new Stopwatch();
		isGestureEnabled = MySettingsProperty.Settings.isGestureEnabled;
		FrameNoOfBackgroundCollapsed = new int[1];
		FrameNoOfBackgroundPaperAdded = new int[1];
		swQuickAccess = new Stopwatch();
		InitializeComponent();
		CreateByLine();
		SetAboutTexts();
		CheckForUpdates();
	}

	private const string GitHubUrl = "https://github.com/YahyaSvm/Fatih-Kalem-Source";

	private const string CurrentVersion = "2.1.0";

	// Açılıştan sonra arka planda GitHub'daki son sürümü denetler (günde en çok bir kez).
	private void CheckForUpdates()
	{
		string stateDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fatih Kalem");
		string statePath = System.IO.Path.Combine(stateDir, "guncelleme.txt");
		string dismissed = "";
		try
		{
			if (System.IO.File.Exists(statePath))
			{
				string[] state = System.IO.File.ReadAllLines(statePath);
				if (state.Length > 0 && DateTime.TryParse(state[0], out DateTime last) && (DateTime.Now - last).TotalHours < 20.0)
				{
					return;
				}
				if (state.Length > 1)
				{
					dismissed = state[1];
				}
			}
		}
		catch (Exception)
		{
		}
		System.Threading.Tasks.Task.Run(delegate
		{
			try
			{
				System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
				string api = GitHubUrl.Replace("https://github.com/", "https://api.github.com/repos/") + "/releases/latest";
				string json;
				using (System.Net.WebClient client = new System.Net.WebClient())
				{
					client.Headers.Add("User-Agent", "FatihKalem/" + CurrentVersion);
					client.Encoding = System.Text.Encoding.UTF8;
					json = client.DownloadString(api);
				}
				System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(json, "\"tag_name\"\\s*:\\s*\"v?([0-9.]+)\"");
				System.IO.Directory.CreateDirectory(stateDir);
				System.IO.File.WriteAllLines(statePath, new string[2] { DateTime.Now.ToString("o"), dismissed });
				if (!match.Success)
				{
					return;
				}
				string latest = match.Groups[1].Value;
				if (!IsNewerVersion(latest, CurrentVersion) || latest == dismissed)
				{
					return;
				}
				base.Dispatcher.BeginInvoke((Action)delegate
				{
					MessageBoxResult answer = MessageBox.Show(this,
						"Fatih Kalem'in yeni bir sürümü var: " + latest + "\n\nİndirme sayfası açılsın mı?",
						"Fatih Kalem", MessageBoxButton.YesNo, MessageBoxImage.Information);
					if (answer == MessageBoxResult.Yes)
					{
						Process.Start(GitHubUrl + "/releases/latest");
					}
					else
					{
						try
						{
							System.IO.File.WriteAllLines(statePath, new string[2] { DateTime.Now.ToString("o"), latest });
						}
						catch (Exception)
						{
						}
					}
				});
			}
			catch (Exception)
			{
			}
		});
	}

	private static bool IsNewerVersion(string remote, string local)
	{
		Version a;
		Version b;
		return Version.TryParse(remote.Trim('.'), out a) && Version.TryParse(local, out b) && a > b;
	}

	// Ayarlar > Hakkında: sürüm, geliştirici ve GitHub bağlantısı.
	private void SetAboutTexts()
	{
		lblVersion.Content = "Sürüm 2.1";
		lblWebLink.Content = "GitHub Sayfası";
		textBlockAbout.Inlines.Clear();
		textBlockAbout.Inlines.Add(new Bold(new Run("Fatih Kalem 2.1")));
		textBlockAbout.Inlines.Add(new LineBreak());
		textBlockAbout.Inlines.Add(new Run("Etkileşimli tahtalar için kalem programının yeni sürümü. Windows ve Pardus / Linux için geliştirilmektedir."));
		textBlockAbout.Inlines.Add(new LineBreak());
		textBlockAbout.Inlines.Add(new LineBreak());
		textBlockAbout.Inlines.Add(new Bold(new Run("Geliştiren: Yahya Eren Sevim (YhySvm)")));
		textBlockAbout.Inlines.Add(new LineBreak());
		textBlockAbout.Inlines.Add(new Run("github.com/YahyaSvm/Fatih-Kalem-Source"));
		textBlockAbout.Inlines.Add(new LineBreak());
		textBlockAbout.Inlines.Add(new LineBreak());
		textBlockAbout.Inlines.Add(new Run("İlk Fatih Kalem (1.0): Hasan Yunus ATEŞ, MEB YEĞİTEK.")
		{
			FontSize = textBlockAbout.FontSize * 0.85
		});
	}

	private const int ByLineHeight = 16;

	private TextBlock txtByLine;

	// Ana menü açıldığında (büyüdüğünde) en altta görünen imza.
	private void CreateByLine()
	{
		txtByLine = new TextBlock
		{
			Text = "By YhySvm",
			FontSize = 8.0,
			FontWeight = FontWeights.SemiBold,
			TextTrimming = TextTrimming.None,
			Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 110, 110, 110)),
			TextAlignment = TextAlignment.Center,
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Top,
			Height = ByLineHeight,
			IsHitTestVisible = false,
			Visibility = Visibility.Collapsed
		};
		gridMainMenu.Children.Add(txtByLine);
	}

	private void ShowByLine(bool isVisible)
	{
		if (txtByLine == null)
		{
			return;
		}
		if (isVisible)
		{
			txtByLine.Width = borderMainMenu.Width;
			txtByLine.Margin = new Thickness(borderMainMenu.Margin.Left, borderMainMenu.Margin.Top + borderMainMenu.Height - ByLineHeight - 1.0, 0.0, 0.0);
			txtByLine.Visibility = Visibility.Visible;
		}
		else
		{
			txtByLine.Visibility = Visibility.Collapsed;
		}
	}
	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		int num5 = default(int);
		int num6 = default(int);
		int num7 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (!DebugMode)
						{
							goto IL_000a;
						}
						goto IL_0013;
					case 1196:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0013;
							case 4:
								goto IL_001d;
							case 5:
								goto IL_0024;
							case 6:
								goto IL_003e;
							case 7:
								goto IL_0046;
							case 8:
								goto IL_0058;
							case 9:
								goto IL_0066;
							case 10:
								goto IL_0079;
							case 11:
								goto IL_008c;
							case 12:
								goto IL_009f;
							case 13:
								goto IL_00b2;
							case 14:
								goto IL_00c5;
							case 15:
								goto IL_00d8;
							case 16:
								goto IL_00e1;
							case 17:
								goto IL_00fa;
							case 18:
								goto IL_0104;
							case 19:
								goto IL_010d;
							case 20:
								goto IL_011f;
							case 21:
								goto IL_0128;
							case 22:
								goto IL_013a;
							case 23:
								goto IL_016c;
							case 24:
								goto IL_01a2;
							case 26:
								goto IL_01d1;
							case 27:
								goto IL_01df;
							case 28:
								goto IL_01e6;
							case 29:
								goto IL_01eb;
							case 30:
								goto IL_0204;
							case 31:
								goto IL_0220;
							case 32:
								goto IL_0241;
							case 33:
								goto IL_0250;
							case 34:
								goto IL_0258;
							case 35:
								goto IL_025e;
							case 36:
								goto IL_0278;
							case 37:
								goto IL_0295;
							case 25:
							case 38:
								goto IL_02bf;
							case 39:
								goto IL_02c8;
							case 40:
								goto IL_02d1;
							case 41:
								goto IL_02e5;
							case 42:
								goto IL_02fb;
							case 43:
								goto IL_0316;
							case 44:
								goto IL_0320;
							case 45:
								goto IL_032b;
							case 46:
								goto IL_0336;
							case 47:
								goto IL_0340;
							case 48:
								goto IL_0352;
							case 49:
								goto IL_035d;
							case 50:
								goto IL_0367;
							case 51:
								goto IL_0386;
							case 52:
								goto IL_03a0;
							case 54:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 53:
							case 55:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_02fb:
						num2 = 42;
						i++;
						goto IL_030c;
						IL_000a:
						num2 = 2;
						IgnoreErrors = true;
						goto IL_0013;
						IL_0013:
						num2 = 3;
						if (IgnoreErrors)
						{
							goto IL_001d;
						}
						goto IL_0024;
						IL_001d:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0024;
						IL_0024:
						num2 = 5;
						Module1.Splash1.Close(TimeSpan.FromMilliseconds(1.0));
						goto IL_003e;
						IL_003e:
						num2 = 6;
						CheckPrevinstanceAndCommandLineArguments();
						goto IL_0046;
						IL_0046:
						num2 = 7;
						PenStyle = unchecked((PenStyleEnum)MySettingsProperty.Settings.PenStyle);
						goto IL_0058;
						IL_0058:
						num2 = 8;
						ActiveTab = PenStyle;
						goto IL_0066;
						IL_0066:
						num2 = 9;
						ColorNo = MySettingsProperty.Settings.ColorNo;
						goto IL_0079;
						IL_0079:
						num2 = 10;
						InkColor = MySettingsProperty.Settings.InkColor;
						goto IL_008c;
						IL_008c:
						num2 = 11;
						InkSize = MySettingsProperty.Settings.InkSize;
						goto IL_009f;
						IL_009f:
						num2 = 12;
						isSideArrowsEnabled = MySettingsProperty.Settings.isSideArrowsEnabled;
						goto IL_00b2;
						IL_00b2:
						num2 = 13;
						isShortGestureEnabled = MySettingsProperty.Settings.isShortGestureEnabled;
						goto IL_00c5;
						IL_00c5:
						num2 = 14;
						iFav = MySettingsProperty.Settings.iFav;
						goto IL_00d8;
						IL_00d8:
						num2 = 15;
						ActivatePen();
						goto IL_00e1;
						IL_00e1:
						num2 = 16;
						inkCanvas.DefaultDrawingAttributes.Color = InkColor;
						goto IL_00fa;
						IL_00fa:
						num2 = 17;
						ChangeInkSize(0);
						goto IL_0104;
						IL_0104:
						num2 = 18;
						ChangeInkSizeFrame();
						goto IL_010d;
						IL_010d:
						num2 = 19;
						base.Opacity = 0.0;
						goto IL_011f;
						IL_011f:
						num2 = 20;
						SetWindowSize();
						goto IL_0128;
						IL_0128:
						num2 = 21;
						if (MySettingsProperty.Settings.isStartupPositionDefault)
						{
							goto IL_013a;
						}
						goto IL_01d1;
						IL_013a:
						num2 = 22;
						num5 = (int)Math.Round(0.0 - borderMainMenu.Margin.Left + 8.0);
						goto IL_016c;
						IL_016c:
						num2 = 23;
						num6 = (int)Math.Round((base.Height - (double)(223 + iFav * 42)) * 1.0 / 3.0);
						goto IL_01a2;
						IL_01a2:
						num2 = 24;
						cnvMainMenu.Margin = new Thickness(num5, num6, 0.0, 0.0);
						goto IL_02bf;
						IL_01d1:
						num2 = 26;
						num5 = MySettingsProperty.Settings.Left;
						goto IL_01df;
						IL_01df:
						num2 = 27;
						if (num5 < 0)
						{
							goto IL_01e6;
						}
						goto IL_01eb;
						IL_01e6:
						num2 = 28;
						num5 = 0;
						goto IL_01eb;
						IL_01eb:
						num2 = 29;
						if ((double)num5 > base.Width - borderMainMenu.Width)
						{
							goto IL_0204;
						}
						goto IL_0220;
						IL_0204:
						num2 = 30;
						num5 = (int)Math.Round(base.Width - borderMainMenu.Width);
						goto IL_0220;
						IL_0220:
						num2 = 31;
						num5 = (int)Math.Round((double)num5 - borderMainMenu.Margin.Left);
						goto IL_0241;
						IL_0241:
						num2 = 32;
						num6 = MySettingsProperty.Settings.Top;
						goto IL_0250;
						IL_0250:
						num2 = 33;
						if (num6 < 0)
						{
							goto IL_0258;
						}
						goto IL_025e;
						IL_0258:
						num2 = 34;
						num6 = 0;
						goto IL_025e;
						IL_025e:
						num2 = 35;
						if ((double)num6 > base.Height - borderMainMenu.Height)
						{
							goto IL_0278;
						}
						goto IL_0295;
						IL_0278:
						num2 = 36;
						num6 = (int)Math.Round(base.Height - borderMainMenu.Height);
						goto IL_0295;
						IL_0295:
						num2 = 37;
						cnvMainMenu.Margin = new Thickness(num5, num6, 0.0, 0.0);
						goto IL_02bf;
						IL_02bf:
						num2 = 38;
						InitTimers();
						goto IL_02c8;
						IL_02c8:
						num2 = 39;
						LoadFavourites();
						goto IL_02d1;
						IL_02d1:
						num2 = 40;
						num7 = iFav;
						i = 1;
						goto IL_030c;
						IL_030c:
						if (i <= num7)
						{
							goto IL_02e5;
						}
						goto IL_0316;
						IL_0316:
						num2 = 43;
						isMinimized = true;
						goto IL_0320;
						IL_0320:
						num2 = 44;
						imgHandAndPen_PreviewMouseLeftButtonUp(null, null);
						goto IL_032b;
						IL_032b:
						num2 = 45;
						imgHandAndPen_PreviewMouseLeftButtonUp(null, null);
						goto IL_0336;
						IL_0336:
						num2 = 46;
						isFirstRun = false;
						goto IL_0340;
						IL_0340:
						num2 = 47;
						base.Opacity = 1.0;
						goto IL_0352;
						IL_0352:
						num2 = 48;
						if (!DebugMode)
						{
							break;
						}
						goto IL_035d;
						IL_035d:
						num2 = 49;
						base.Topmost = false;
						goto IL_0367;
						IL_0367:
						num2 = 50;
						tmrDebug = new DispatcherTimer
						{
							Interval = new TimeSpan(0, 0, 0, 0, 100)
						};
						goto IL_0386;
						IL_0386:
						num2 = 51;
						tmrDebug.Tick += tmrDebug_Tick;
						goto IL_03a0;
						IL_03a0:
						num2 = 52;
						tmrDebug.IsEnabled = true;
						goto end_IL_0000_3;
						IL_02e5:
						num2 = 41;
						imgFav[i].Visibility = Visibility.Collapsed;
						goto IL_02fb;
						end_IL_0000_2:
						break;
					}
					num2 = 54;
					base.Topmost = true;
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1196;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MainWindow_Activated(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 74:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0011:
					num2 = 3;
					if (DebugMode)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					end_IL_0000_2:
					break;
				}
				num2 = 4;
				base.Topmost = true;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 74;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MainWindow_Deactivated(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 74:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0011:
					num2 = 3;
					if (DebugMode)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					end_IL_0000_2:
					break;
				}
				num2 = 4;
				base.Topmost = true;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 74;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MyBase_Deactivated(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 74:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0011:
					num2 = 3;
					if (DebugMode)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					end_IL_0000_2:
					break;
				}
				num2 = 4;
				base.Topmost = true;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 74;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MainWindow_StateChanged(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 74:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0011:
					num2 = 3;
					if (base.WindowState == WindowState.Normal)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					end_IL_0000_2:
					break;
				}
				num2 = 4;
				base.WindowState = WindowState.Normal;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 74;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgTitleBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point position = default(Point);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 330:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001f;
						case 5:
							goto IL_0028;
						case 6:
							goto IL_0037;
						case 7:
							goto IL_0060;
						case 8:
							goto IL_0089;
						case 9:
							goto IL_0093;
						case 10:
							goto IL_009c;
						case 11:
							goto IL_00be;
						case 12:
							goto IL_00e0;
						case 13:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 14:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00e0:
					num2 = 12;
					swVelocity.Start();
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					Mouse.Capture(imgTitleBar);
					goto IL_001f;
					IL_001f:
					num2 = 4;
					DragMainControl = true;
					goto IL_0028;
					IL_0028:
					num2 = 5;
					position = e.GetPosition(imgTitleBar);
					goto IL_0037;
					IL_0037:
					num2 = 6;
					Drag.X = position.X + borderMainMenu.Margin.Left;
					goto IL_0060;
					IL_0060:
					num2 = 7;
					Drag.Y = position.Y + borderMainMenu.Margin.Top;
					goto IL_0089;
					IL_0089:
					num2 = 8;
					if (ShownSubMenu != null)
					{
						goto IL_0093;
					}
					goto IL_009c;
					IL_0093:
					num2 = 9;
					CollapseSubMenus();
					goto IL_009c;
					IL_009c:
					num2 = 10;
					pOldPos.X = cnvMainMenu.Margin.Left;
					goto IL_00be;
					IL_00be:
					num2 = 11;
					pOldPos.Y = cnvMainMenu.Margin.Top;
					goto IL_00e0;
					end_IL_0000_2:
					break;
				}
				num2 = 13;
				tmrVelocity.IsEnabled = true;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 330;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgTitleBar_PreviewMouseMove(object sender, MouseEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point position = default(Point);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 187:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001b;
						case 5:
							goto IL_0026;
						case 6:
							goto IL_0030;
						case 8:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 7:
						case 9:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0030:
					num2 = 6;
					cnvMainMenu.Margin = new Thickness(position.X - Drag.X, position.Y - Drag.Y, 0.0, 0.0);
					goto end_IL_0000_3;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (!DragMainControl)
					{
						goto end_IL_0000_3;
					}
					goto IL_001b;
					IL_001b:
					num2 = 4;
					if (e.LeftButton != MouseButtonState.Pressed)
					{
						break;
					}
					goto IL_0026;
					IL_0026:
					num2 = 5;
					position = e.GetPosition(this);
					goto IL_0030;
					end_IL_0000_2:
					break;
				}
				num2 = 8;
				DragMainControl = false;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 187;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgTitleBar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 415:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001a;
						case 5:
							goto IL_0023;
						case 6:
							goto IL_0054;
						case 7:
							goto IL_005e;
						case 8:
							goto IL_007e;
						case 9:
							goto IL_009e;
						case 10:
							goto IL_00bb;
						case 11:
							goto IL_00ca;
						case 12:
							goto IL_00f8;
						case 13:
							goto IL_0107;
						case 14:
							goto IL_0124;
						case 16:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 15:
						case 17:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0124:
					num2 = 14;
					tFirst = 0.0;
					goto end_IL_0000_3;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					DragMainControl = false;
					goto IL_001a;
					IL_001a:
					num2 = 4;
					Mouse.Capture(null);
					goto IL_0023;
					IL_0023:
					num2 = 5;
					if ((Math.Abs(vX) < 0.1) | (Math.Abs(vY) < 0.1))
					{
						goto IL_0054;
					}
					goto IL_005e;
					IL_0054:
					num2 = 6;
					tmrVelocity_Tick(null, null);
					goto IL_005e;
					IL_005e:
					num2 = 7;
					pOldPos.X = cnvMainMenu.Margin.Left;
					goto IL_007e;
					IL_007e:
					num2 = 8;
					pOldPos.Y = cnvMainMenu.Margin.Top;
					goto IL_009e;
					IL_009e:
					num2 = 9;
					tFirst = swVelocity.Elapsed.TotalMilliseconds;
					goto IL_00bb;
					IL_00bb:
					num2 = 10;
					tmrVelocity.IsEnabled = false;
					goto IL_00ca;
					IL_00ca:
					num2 = 11;
					if (!((vX != 0.0) | (vY != 0.0)))
					{
						break;
					}
					goto IL_00f8;
					IL_00f8:
					num2 = 12;
					tmrAcceleration.IsEnabled = true;
					goto IL_0107;
					IL_0107:
					num2 = 13;
					tStart = swVelocity.Elapsed.TotalMilliseconds;
					goto IL_0124;
					end_IL_0000_2:
					break;
				}
				num2 = 16;
				swVelocity.Stop();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 415;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void tmrVelocity_Tick(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point point = default(Point);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 276:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_002e;
						case 5:
							goto IL_004b;
						case 6:
							goto IL_0082;
						case 7:
							goto IL_00b9;
						case 8:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 9:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00b9:
					num2 = 7;
					tFirst = swVelocity.Elapsed.TotalMilliseconds;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					point.X = cnvMainMenu.Margin.Left;
					goto IL_002e;
					IL_002e:
					num2 = 4;
					point.Y = cnvMainMenu.Margin.Top;
					goto IL_004b;
					IL_004b:
					num2 = 5;
					vX = (point.X - pOldPos.X) / (swVelocity.Elapsed.TotalMilliseconds - tFirst);
					goto IL_0082;
					IL_0082:
					num2 = 6;
					vY = (point.Y - pOldPos.Y) / (swVelocity.Elapsed.TotalMilliseconds - tFirst);
					goto IL_00b9;
					end_IL_0000_2:
					break;
				}
				num2 = 8;
				pOldPos = point;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 276;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void tmrAcceleration_Tick(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double num5 = default(double);
		double y = default(double);
		double num6 = default(double);
		double num7 = default(double);
		int num8 = default(int);
		int num9 = default(int);
		int num10 = default(int);
		bool flag = default(bool);
		int num11 = default(int);
		int num12 = default(int);
		int num13 = default(int);
		bool flag2 = default(bool);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1882:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_0030;
							case 5:
								goto IL_003d;
							case 6:
								goto IL_004a;
							case 7:
								goto IL_0057;
							case 8:
								goto IL_0076;
							case 9:
								goto IL_0095;
							case 10:
								goto IL_00ac;
							case 11:
								goto IL_00de;
							case 12:
								goto IL_0118;
							case 13:
								goto IL_0122;
							case 14:
								goto IL_012a;
							case 15:
								goto IL_0130;
							case 16:
								goto IL_0135;
							case 17:
								goto IL_017c;
							case 18:
								goto IL_01dc;
							case 19:
								goto IL_01e2;
							case 21:
								goto IL_01f7;
							case 22:
								goto IL_01ff;
							case 23:
								goto IL_0231;
							case 24:
								goto IL_026b;
							case 25:
								goto IL_0275;
							case 26:
								goto IL_027d;
							case 27:
								goto IL_0283;
							case 28:
								goto IL_0288;
							case 29:
								goto IL_02c3;
							case 30:
								goto IL_0311;
							case 31:
								goto IL_0317;
							case 20:
							case 32:
								goto IL_0327;
							case 33:
								goto IL_033e;
							case 34:
								goto IL_0370;
							case 35:
								goto IL_03aa;
							case 36:
								goto IL_03b4;
							case 37:
								goto IL_03bc;
							case 38:
								goto IL_03c2;
							case 39:
								goto IL_03c8;
							case 40:
								goto IL_040f;
							case 41:
								goto IL_046f;
							case 42:
								goto IL_0475;
							case 44:
								goto IL_048a;
							case 45:
								goto IL_0492;
							case 46:
								goto IL_04c4;
							case 47:
								goto IL_04fe;
							case 48:
								goto IL_0508;
							case 49:
								goto IL_0510;
							case 50:
								goto IL_0516;
							case 51:
								goto IL_051c;
							case 52:
								goto IL_0557;
							case 53:
								goto IL_05b9;
							case 54:
								goto IL_05bf;
							case 43:
							case 55:
								goto IL_05cf;
							case 56:
								goto IL_0624;
							case 57:
								goto IL_062d;
							case 58:
								goto IL_063c;
							case 59:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 60:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_063c:
						num2 = 58;
						swVelocity.Stop();
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						num5 = swVelocity.Elapsed.TotalMilliseconds - tStart;
						goto IL_0030;
						IL_0030:
						num2 = 4;
						y = num5 - tFirst;
						goto IL_003d;
						IL_003d:
						num2 = 5;
						num6 = 0.0;
						goto IL_004a;
						IL_004a:
						num2 = 6;
						num7 = 0.0;
						goto IL_0057;
						IL_0057:
						num2 = 7;
						vX *= Math.Pow(0.998, y);
						goto IL_0076;
						IL_0076:
						num2 = 8;
						vY *= Math.Pow(0.998, y);
						goto IL_0095;
						IL_0095:
						num2 = 9;
						if (vX > 0.0)
						{
							goto IL_00ac;
						}
						goto IL_01f7;
						IL_00ac:
						num2 = 10;
						num8 = (int)Math.Round(vX * num5 - 0.5 * num6 * Math.Pow(num5, 2.0));
						goto IL_00de;
						IL_00de:
						num2 = 11;
						num9 = (int)Math.Round(vX * tFirst - 0.5 * num6 * Math.Pow(tFirst, 2.0));
						goto IL_0118;
						IL_0118:
						num2 = 12;
						num10 = num8 - num9;
						goto IL_0122;
						IL_0122:
						num2 = 13;
						if (num10 <= 0)
						{
							goto IL_012a;
						}
						goto IL_0135;
						IL_012a:
						num2 = 14;
						num10 = 0;
						goto IL_0130;
						IL_0130:
						num2 = 15;
						flag = true;
						goto IL_0135;
						IL_0135:
						num2 = 16;
						if (cnvMainMenu.Margin.Left + (double)num10 + borderMainMenu.Margin.Left + borderMainMenu.Width > base.Width)
						{
							goto IL_017c;
						}
						goto IL_0327;
						IL_017c:
						num2 = 17;
						cnvMainMenu.Margin = new Thickness(base.Width - borderMainMenu.Margin.Left - borderMainMenu.Width, cnvMainMenu.Margin.Top, 0.0, 0.0);
						goto IL_01dc;
						IL_01dc:
						num2 = 18;
						num10 = 1;
						goto IL_01e2;
						IL_01e2:
						num2 = 19;
						vX = 0.0 - vX;
						goto IL_0327;
						IL_01f7:
						num2 = 21;
						num6 = 0.0 - num6;
						goto IL_01ff;
						IL_01ff:
						num2 = 22;
						num8 = (int)Math.Round(vX * num5 - 0.5 * num6 * Math.Pow(num5, 2.0));
						goto IL_0231;
						IL_0231:
						num2 = 23;
						num9 = (int)Math.Round(vX * tFirst - 0.5 * num6 * Math.Pow(tFirst, 2.0));
						goto IL_026b;
						IL_026b:
						num2 = 24;
						num10 = num8 - num9;
						goto IL_0275;
						IL_0275:
						num2 = 25;
						if (num10 >= 0)
						{
							goto IL_027d;
						}
						goto IL_0288;
						IL_027d:
						num2 = 26;
						num10 = 0;
						goto IL_0283;
						IL_0283:
						num2 = 27;
						flag = true;
						goto IL_0288;
						IL_0288:
						num2 = 28;
						if (cnvMainMenu.Margin.Left + (double)num10 + borderMainMenu.Margin.Left < 0.0)
						{
							goto IL_02c3;
						}
						goto IL_0327;
						IL_02c3:
						num2 = 29;
						cnvMainMenu.Margin = new Thickness(0.0 - borderMainMenu.Margin.Left, cnvMainMenu.Margin.Top, 0.0, 0.0);
						goto IL_0311;
						IL_0311:
						num2 = 30;
						num10 = 1;
						goto IL_0317;
						IL_0317:
						num2 = 31;
						vX = 0.0 - vX;
						goto IL_0327;
						IL_0327:
						num2 = 32;
						if (vY > 0.0)
						{
							goto IL_033e;
						}
						goto IL_048a;
						IL_033e:
						num2 = 33;
						num11 = (int)Math.Round(vY * num5 - 0.5 * num7 * Math.Pow(num5, 2.0));
						goto IL_0370;
						IL_0370:
						num2 = 34;
						num12 = (int)Math.Round(vY * tFirst - 0.5 * num7 * Math.Pow(tFirst, 2.0));
						goto IL_03aa;
						IL_03aa:
						num2 = 35;
						num13 = num11 - num12;
						goto IL_03b4;
						IL_03b4:
						num2 = 36;
						if (num13 <= 0)
						{
							goto IL_03bc;
						}
						goto IL_03c8;
						IL_03bc:
						num2 = 37;
						num13 = 0;
						goto IL_03c2;
						IL_03c2:
						num2 = 38;
						flag2 = true;
						goto IL_03c8;
						IL_03c8:
						num2 = 39;
						if (cnvMainMenu.Margin.Top + (double)num13 + borderMainMenu.Margin.Top + borderMainMenu.Height > base.Height)
						{
							goto IL_040f;
						}
						goto IL_05cf;
						IL_040f:
						num2 = 40;
						cnvMainMenu.Margin = new Thickness(cnvMainMenu.Margin.Left, base.Height - borderMainMenu.Margin.Top - borderMainMenu.Height, 0.0, 0.0);
						goto IL_046f;
						IL_046f:
						num2 = 41;
						num13 = 1;
						goto IL_0475;
						IL_0475:
						num2 = 42;
						vY = 0.0 - vY;
						goto IL_05cf;
						IL_048a:
						num2 = 44;
						num7 = 0.0 - num7;
						goto IL_0492;
						IL_0492:
						num2 = 45;
						num11 = (int)Math.Round(vY * num5 - 0.5 * num7 * Math.Pow(num5, 2.0));
						goto IL_04c4;
						IL_04c4:
						num2 = 46;
						num12 = (int)Math.Round(vY * tFirst - 0.5 * num7 * Math.Pow(tFirst, 2.0));
						goto IL_04fe;
						IL_04fe:
						num2 = 47;
						num13 = num11 - num12;
						goto IL_0508;
						IL_0508:
						num2 = 48;
						if (num13 >= 0)
						{
							goto IL_0510;
						}
						goto IL_051c;
						IL_0510:
						num2 = 49;
						num13 = 0;
						goto IL_0516;
						IL_0516:
						num2 = 50;
						flag2 = true;
						goto IL_051c;
						IL_051c:
						num2 = 51;
						if (cnvMainMenu.Margin.Top + (double)num13 + borderMainMenu.Margin.Top < 0.0)
						{
							goto IL_0557;
						}
						goto IL_05cf;
						IL_0557:
						num2 = 52;
						cnvMainMenu.Margin = new Thickness(cnvMainMenu.Margin.Left, cnvMainMenu.Margin.Top - borderMainMenu.Margin.Top, 0.0, 0.0);
						goto IL_05b9;
						IL_05b9:
						num2 = 53;
						num13 = 1;
						goto IL_05bf;
						IL_05bf:
						num2 = 54;
						vY = 0.0 - vY;
						goto IL_05cf;
						IL_05cf:
						num2 = 55;
						cnvMainMenu.Margin = new Thickness(cnvMainMenu.Margin.Left + (double)num10, cnvMainMenu.Margin.Top + (double)num13, 0.0, 0.0);
						goto IL_0624;
						IL_0624:
						num2 = 56;
						if (!unchecked(flag && flag2))
						{
							break;
						}
						goto IL_062d;
						IL_062d:
						num2 = 57;
						tmrAcceleration.IsEnabled = false;
						goto IL_063c;
						end_IL_0000_2:
						break;
					}
					num2 = 59;
					tFirst = num5;
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1882;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgSideArrowLeft_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 504:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_003a;
						case 5:
							goto IL_0048;
						case 7:
							goto IL_005b;
						case 8:
							goto IL_0068;
						case 9:
							goto IL_0076;
						case 10:
							goto IL_0084;
						case 11:
							goto IL_00a1;
						case 12:
							goto IL_00b3;
						case 13:
							goto IL_00f9;
						case 15:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 6:
						case 14:
						case 16:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00f9:
					num2 = 13;
					vX = 4.9 * (0.0 - base.Width + 0.0 - (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left)) / 1920.0;
					goto end_IL_0000_3;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (DateTime.Now.Subtract(tLastClick).TotalMilliseconds < 1000.0)
					{
						goto IL_003a;
					}
					goto IL_005b;
					IL_003a:
					num2 = 4;
					imgSideArrowLeft.Visibility = Visibility.Collapsed;
					goto IL_0048;
					IL_0048:
					num2 = 5;
					tmrReShowSideArrowLeft.IsEnabled = true;
					goto end_IL_0000_3;
					IL_005b:
					num2 = 7;
					tLastClick = DateTime.Now;
					goto IL_0068;
					IL_0068:
					num2 = 8;
					tmrSideArrows.IsEnabled = true;
					goto IL_0076;
					IL_0076:
					num2 = 9;
					swQuickAccess.Start();
					goto IL_0084;
					IL_0084:
					num2 = 10;
					tStart = swQuickAccess.Elapsed.TotalMilliseconds;
					goto IL_00a1;
					IL_00a1:
					num2 = 11;
					tFirst = 0.0;
					goto IL_00b3;
					IL_00b3:
					num2 = 12;
					if (!(Math.Abs(0.0 - (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left)) < 100.0))
					{
						break;
					}
					goto IL_00f9;
					end_IL_0000_2:
					break;
				}
				num2 = 15;
				vX = 4.9 * (0.0 - (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left)) / 1920.0;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 504;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgSideArrowRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 525:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_003a;
						case 5:
							goto IL_0048;
						case 7:
							goto IL_005b;
						case 8:
							goto IL_0068;
						case 9:
							goto IL_0076;
						case 10:
							goto IL_0084;
						case 11:
							goto IL_00a1;
						case 12:
							goto IL_00b3;
						case 13:
							goto IL_00fd;
						case 15:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 6:
						case 14:
						case 16:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00fd:
					num2 = 13;
					vX = 4.9 * (base.Width + base.Width - (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left + borderMainMenu.Width)) / 1920.0;
					goto end_IL_0000_3;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (DateTime.Now.Subtract(tLastClick).TotalMilliseconds < 1000.0)
					{
						goto IL_003a;
					}
					goto IL_005b;
					IL_003a:
					num2 = 4;
					imgSideArrowRight.Visibility = Visibility.Collapsed;
					goto IL_0048;
					IL_0048:
					num2 = 5;
					tmrReShowSideArrowRight.IsEnabled = true;
					goto end_IL_0000_3;
					IL_005b:
					num2 = 7;
					tLastClick = DateTime.Now;
					goto IL_0068;
					IL_0068:
					num2 = 8;
					tmrSideArrows.IsEnabled = true;
					goto IL_0076;
					IL_0076:
					num2 = 9;
					swQuickAccess.Start();
					goto IL_0084;
					IL_0084:
					num2 = 10;
					tStart = swQuickAccess.Elapsed.TotalMilliseconds;
					goto IL_00a1;
					IL_00a1:
					num2 = 11;
					tFirst = 0.0;
					goto IL_00b3;
					IL_00b3:
					num2 = 12;
					if (!(base.Width - (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left + borderMainMenu.Width) < 100.0))
					{
						break;
					}
					goto IL_00fd;
					end_IL_0000_2:
					break;
				}
				num2 = 15;
				vX = 4.9 * (base.Width - (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left + borderMainMenu.Width)) / 1920.0;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 525;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void tmrSideArrows_Tick(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		int num5 = default(int);
		double num6 = default(double);
		double y = default(double);
		double num7 = default(double);
		int num8 = default(int);
		int num9 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1054:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_0030;
							case 5:
								goto IL_003d;
							case 6:
								goto IL_004a;
							case 7:
								goto IL_0069;
							case 8:
								goto IL_007f;
							case 9:
								goto IL_00af;
							case 10:
								goto IL_00e9;
							case 11:
								goto IL_00f2;
							case 12:
								goto IL_00fa;
							case 13:
								goto IL_0100;
							case 14:
								goto IL_010f;
							case 15:
								goto IL_011d;
							case 16:
								goto IL_0164;
							case 17:
								goto IL_01c4;
							case 18:
								goto IL_01ca;
							case 20:
								goto IL_01df;
							case 21:
								goto IL_01e7;
							case 22:
								goto IL_0218;
							case 23:
								goto IL_0252;
							case 24:
								goto IL_025b;
							case 25:
								goto IL_0263;
							case 26:
								goto IL_0269;
							case 27:
								goto IL_0278;
							case 28:
								goto IL_0286;
							case 29:
								goto IL_02c1;
							case 30:
								goto IL_030f;
							case 31:
								goto IL_0315;
							case 19:
							case 32:
								goto IL_0325;
							case 33:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 34:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_0325:
						num2 = 32;
						cnvMainMenu.Margin = new Thickness(cnvMainMenu.Margin.Left + (double)num5, cnvMainMenu.Margin.Top, 0.0, 0.0);
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						num6 = swQuickAccess.Elapsed.TotalMilliseconds - tStart;
						goto IL_0030;
						IL_0030:
						num2 = 4;
						y = num6 - tFirst;
						goto IL_003d;
						IL_003d:
						num2 = 5;
						num7 = 0.0;
						goto IL_004a;
						IL_004a:
						num2 = 6;
						vX *= Math.Pow(0.9976, y);
						goto IL_0069;
						IL_0069:
						num2 = 7;
						if (vX > 0.0)
						{
							goto IL_007f;
						}
						goto IL_01df;
						IL_007f:
						num2 = 8;
						num8 = (int)Math.Round(vX * num6 - 0.5 * num7 * Math.Pow(num6, 2.0));
						goto IL_00af;
						IL_00af:
						num2 = 9;
						num9 = (int)Math.Round(vX * tFirst - 0.5 * num7 * Math.Pow(tFirst, 2.0));
						goto IL_00e9;
						IL_00e9:
						num2 = 10;
						num5 = num8 - num9;
						goto IL_00f2;
						IL_00f2:
						num2 = 11;
						if (num5 <= 0)
						{
							goto IL_00fa;
						}
						goto IL_011d;
						IL_00fa:
						num2 = 12;
						num5 = 0;
						goto IL_0100;
						IL_0100:
						num2 = 13;
						tmrSideArrows.IsEnabled = false;
						goto IL_010f;
						IL_010f:
						num2 = 14;
						swQuickAccess.Stop();
						goto IL_011d;
						IL_011d:
						num2 = 15;
						if ((double)num5 + (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left + borderMainMenu.Width) > base.Width)
						{
							goto IL_0164;
						}
						goto IL_0325;
						IL_0164:
						num2 = 16;
						cnvMainMenu.Margin = new Thickness(base.Width - borderMainMenu.Margin.Left - borderMainMenu.Width, cnvMainMenu.Margin.Top, 0.0, 0.0);
						goto IL_01c4;
						IL_01c4:
						num2 = 17;
						num5 = 1;
						goto IL_01ca;
						IL_01ca:
						num2 = 18;
						vX = 0.0 - vX;
						goto IL_0325;
						IL_01df:
						num2 = 20;
						num7 = 0.0 - num7;
						goto IL_01e7;
						IL_01e7:
						num2 = 21;
						num8 = (int)Math.Round(vX * num6 - 0.5 * num7 * Math.Pow(num6, 2.0));
						goto IL_0218;
						IL_0218:
						num2 = 22;
						num9 = (int)Math.Round(vX * tFirst - 0.5 * num7 * Math.Pow(tFirst, 2.0));
						goto IL_0252;
						IL_0252:
						num2 = 23;
						num5 = num8 - num9;
						goto IL_025b;
						IL_025b:
						num2 = 24;
						if (num5 >= 0)
						{
							goto IL_0263;
						}
						goto IL_0286;
						IL_0263:
						num2 = 25;
						num5 = 0;
						goto IL_0269;
						IL_0269:
						num2 = 26;
						tmrSideArrows.IsEnabled = false;
						goto IL_0278;
						IL_0278:
						num2 = 27;
						swQuickAccess.Stop();
						goto IL_0286;
						IL_0286:
						num2 = 28;
						if ((double)num5 + (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left) < 0.0)
						{
							goto IL_02c1;
						}
						goto IL_0325;
						IL_02c1:
						num2 = 29;
						cnvMainMenu.Margin = new Thickness(0.0 - borderMainMenu.Margin.Left, cnvMainMenu.Margin.Top, 0.0, 0.0);
						goto IL_030f;
						IL_030f:
						num2 = 30;
						num5 = 1;
						goto IL_0315;
						IL_0315:
						num2 = 31;
						vX = 0.0 - vX;
						goto IL_0325;
						end_IL_0000_2:
						break;
					}
					num2 = 33;
					tFirst = num6;
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1054;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void tmrReShowSideArrowRight_Tick(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 106:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0011:
					num2 = 3;
					tmrReShowSideArrowRight.IsEnabled = false;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					end_IL_0000_2:
					break;
				}
				num2 = 4;
				AnimationFadeIn(imgSideArrowRight, 500);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 106;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void tmrReShowSideArrowLeft_Tick(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 106:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0011:
					num2 = 3;
					tmrReShowSideArrowLeft.IsEnabled = false;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					end_IL_0000_2:
					break;
				}
				num2 = 4;
				AnimationFadeIn(imgSideArrowLeft, 500);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 106;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MainWindow_PreviewStylusUp(object sender, StylusEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (!IgnoreErrors)
					{
						break;
					}
					goto IL_000a;
				case 66:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 4:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					break;
					end_IL_0000_2:
					break;
				}
				num2 = 3;
				inkCanvas_PreviewStylusUp(RuntimeHelpers.GetObjectValue(sender), e);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 66;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MainWindow_StylusEnter(object sender, StylusEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				int num4;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 159:
					{
						num = num2;
						switch (num3)
						{
						case 2:
							break;
						case 1:
							goto IL_006b;
						default:
							goto end_IL_0000;
						}
						goto IL_0054;
					}
					IL_0011:
					ProjectData.ClearProjectError();
					num3 = 2;
					goto IL_0018;
					IL_0018:
					num2 = 4;
					if (isGestureVisible)
					{
						goto IL_0022;
					}
					goto IL_0054;
					IL_0054:
					num2 = 8;
					ProjectData.ClearProjectError();
					if (num == 0)
					{
						throw ProjectData.CreateProjectError(-2146828268);
					}
					goto IL_006b;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_006b:
					num4 = num + 1;
					num = 0;
					switch (num4)
					{
					case 1:
						break;
					case 2:
						goto IL_000a;
					case 3:
						goto IL_0011;
					case 4:
						goto IL_0018;
					case 5:
						goto IL_0022;
					case 6:
						goto IL_0037;
					case 7:
						goto IL_004c;
					case 8:
						goto IL_0054;
					default:
						goto end_IL_0000;
					case 9:
						goto end_IL_0000_2;
					}
					goto default;
					IL_0022:
					num2 = 5;
					if (e.StylusDevice.Id != LastStylusDeviceID)
					{
						goto IL_0037;
					}
					goto IL_0054;
					IL_0037:
					num2 = 6;
					if (e.StylusDevice.Id == GesturesStylusDeviceID)
					{
						goto IL_004c;
					}
					goto IL_0054;
					IL_004c:
					num2 = 7;
					GestureUp();
					goto IL_0054;
					end_IL_0000:
					break;
				}
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 159;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_2:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MainWindow_StylusLeave(object sender, StylusEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 123:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001b;
						case 5:
							goto IL_0030;
						case 6:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 7:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0030:
					num2 = 5;
					if (e.StylusDevice.Id != GesturesStylusDeviceID)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (!isGestureVisible)
					{
						goto end_IL_0000_3;
					}
					goto IL_001b;
					IL_001b:
					num2 = 4;
					if (e.StylusDevice.Id == LastStylusDeviceID)
					{
						goto end_IL_0000_3;
					}
					goto IL_0030;
					end_IL_0000_2:
					break;
				}
				num2 = 6;
				GestureUp();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 123;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MainWindow_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 73:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0011:
					num2 = 3;
					if (!isGestureVisible)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					end_IL_0000_2:
					break;
				}
				num2 = 4;
				GestureUp();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 73;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MainWindow_MouseEnter(object sender, MouseEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 73:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0011:
					num2 = 3;
					if (!isGestureVisible)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					end_IL_0000_2:
					break;
				}
				num2 = 4;
				GestureUp();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 73;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 73:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0011:
					num2 = 3;
					if (!isGestureVisible)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					end_IL_0000_2:
					break;
				}
				num2 = 4;
				GestureUp();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 73;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void inkCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (DrawState == DrawStateEnum.Pen)
		{
			return;
		}
		if (DrawState == DrawStateEnum.Eraser)
		{
			WaitForEraserMouseUp = false;
		}
		if (!isDraw)
		{
			return;
		}
		switch (DrawState)
		{
		case DrawStateEnum.Line:
			st = null;
			break;
		case DrawStateEnum.DashLine:
			strokeDash = new Stroke[1];
			break;
		case DrawStateEnum.Arrow:
			st = null;
			break;
		case DrawStateEnum.Rectangle:
			st = null;
			break;
		case DrawStateEnum.Ellipse:
			st = null;
			break;
		case DrawStateEnum.Triangle:
			if (!isTrianglesSecondStep)
			{
				isTrianglesSecondStep = true;
				break;
			}
			st2 = null;
			isTrianglesSecondStep = false;
			break;
		case DrawStateEnum.Curtain:
			PlaceCurtain();
			break;
		}
		isDraw = false;
	}

	private void inkCanvas_MouseMove(object sender, MouseEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point position = default(Point);
		long num5 = default(long);
		long num6 = default(long);
		long num7 = default(long);
		long num8 = default(long);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (isGestureVisible & (e.RightButton == MouseButtonState.Pressed))
						{
							goto IL_0014;
						}
						goto IL_0023;
					case 1020:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_0014;
							case 3:
								goto IL_0023;
							case 5:
								goto IL_0030;
							case 6:
								goto IL_003a;
							case 7:
								goto IL_0041;
							case 8:
								goto IL_004b;
							case 9:
								goto IL_0063;
							case 10:
								goto IL_006e;
							case 11:
								goto IL_0088;
							case 12:
								goto IL_0092;
							case 13:
								goto IL_00a4;
							case 14:
								goto IL_00ae;
							case 15:
								goto IL_00b8;
							case 16:
								goto IL_00d0;
							case 17:
								goto IL_00e0;
							case 19:
								goto IL_0119;
							case 21:
								goto IL_012e;
							case 23:
								goto IL_0143;
							case 25:
								goto IL_0158;
							case 27:
								goto IL_016d;
							case 29:
								goto IL_0182;
							case 30:
								goto IL_018d;
							case 31:
								goto IL_01a2;
							case 32:
								goto IL_01b2;
							case 34:
								goto IL_01e2;
							case 35:
								goto IL_01f2;
							case 37:
								goto IL_0224;
							case 38:
								goto IL_023b;
							case 39:
								goto IL_024d;
							case 41:
								goto IL_0265;
							case 42:
								goto IL_0277;
							case 40:
							case 43:
								goto IL_028d;
							case 44:
								goto IL_02a4;
							case 45:
								goto IL_02b6;
							case 47:
								goto IL_02ce;
							case 48:
								goto IL_02e0;
							case 46:
							case 49:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 4:
							case 18:
							case 20:
							case 22:
							case 24:
							case 26:
							case 28:
							case 33:
							case 36:
							case 50:
							case 51:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_0119:
						num2 = 19;
						DrawLine(pStart, position);
						goto end_IL_0000_3;
						IL_0014:
						num2 = 2;
						GestureMove(e.GetPosition(this));
						goto IL_0023;
						IL_0023:
						num2 = 3;
						if (DrawState == DrawStateEnum.Pen)
						{
							goto end_IL_0000_3;
						}
						goto IL_0030;
						IL_0030:
						num2 = 5;
						if (IgnoreErrors)
						{
							goto IL_003a;
						}
						goto IL_0041;
						IL_003a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0041;
						IL_0041:
						num2 = 7;
						if (isGestureVisible)
						{
							goto IL_004b;
						}
						goto IL_00b8;
						IL_004b:
						num2 = 8;
						if (cnvGesture.Opacity > 0.05)
						{
							goto IL_0063;
						}
						goto IL_00b8;
						IL_0063:
						num2 = 9;
						if (st != null)
						{
							goto IL_006e;
						}
						goto IL_00b8;
						IL_006e:
						num2 = 10;
						inkCanvas.Strokes.Remove(st);
						goto IL_0088;
						IL_0088:
						num2 = 11;
						st = null;
						goto IL_0092;
						IL_0092:
						num2 = 12;
						FrameNo--;
						goto IL_00a4;
						IL_00a4:
						num2 = 13;
						isDraw = false;
						goto IL_00ae;
						IL_00ae:
						num2 = 14;
						isTrianglesSecondStep = false;
						goto IL_00b8;
						IL_00b8:
						num2 = 15;
						if (!((e.LeftButton == MouseButtonState.Pressed) & isDraw))
						{
							goto end_IL_0000_3;
						}
						goto IL_00d0;
						IL_00d0:
						num2 = 16;
						position = e.GetPosition(inkCanvas);
						goto IL_00e0;
						IL_00e0:
						num2 = 17;
						switch (DrawState)
						{
						case DrawStateEnum.Line:
							break;
						case DrawStateEnum.DashLine:
							goto IL_012e;
						case DrawStateEnum.Arrow:
							goto IL_0143;
						case DrawStateEnum.Rectangle:
							goto IL_0158;
						case DrawStateEnum.Ellipse:
							goto IL_016d;
						case DrawStateEnum.Triangle:
							goto IL_0182;
						case DrawStateEnum.Curtain:
							goto IL_0224;
						default:
							goto end_IL_0000_3;
						}
						goto IL_0119;
						IL_0224:
						num2 = 37;
						if (position.X <= pStart.X)
						{
							goto IL_023b;
						}
						goto IL_0265;
						IL_023b:
						num2 = 38;
						num5 = (long)Math.Round(position.X);
						goto IL_024d;
						IL_024d:
						num2 = 39;
						num6 = (long)Math.Round(pStart.X);
						goto IL_028d;
						IL_0265:
						num2 = 41;
						num6 = (long)Math.Round(position.X);
						goto IL_0277;
						IL_0277:
						num2 = 42;
						num5 = (long)Math.Round(pStart.X);
						goto IL_028d;
						IL_028d:
						num2 = 43;
						if (position.Y <= pStart.Y)
						{
							goto IL_02a4;
						}
						goto IL_02ce;
						IL_02a4:
						num2 = 44;
						num7 = (long)Math.Round(position.Y);
						goto IL_02b6;
						IL_02b6:
						num2 = 45;
						num8 = (long)Math.Round(pStart.Y);
						break;
						IL_02ce:
						num2 = 47;
						num8 = (long)Math.Round(position.Y);
						goto IL_02e0;
						IL_02e0:
						num2 = 48;
						num7 = (long)Math.Round(pStart.Y);
						break;
						IL_0182:
						num2 = 29;
						if (!isTrianglesSecondStep)
						{
							goto IL_018d;
						}
						goto IL_01e2;
						IL_018d:
						num2 = 30;
						TrianglePoints[1] = pStart;
						goto IL_01a2;
						IL_01a2:
						num2 = 31;
						TrianglePoints[2] = position;
						goto IL_01b2;
						IL_01b2:
						num2 = 32;
						DrawTriangle(TrianglePoints[1], TrianglePoints[2]);
						goto end_IL_0000_3;
						IL_01e2:
						num2 = 34;
						TrianglePoints[3] = position;
						goto IL_01f2;
						IL_01f2:
						num2 = 35;
						DrawTriangle(TrianglePoints[1], TrianglePoints[2], TrianglePoints[3]);
						goto end_IL_0000_3;
						IL_016d:
						num2 = 27;
						DrawEllipse(pStart, position);
						goto end_IL_0000_3;
						IL_0158:
						num2 = 25;
						DrawRectangle(pStart, position);
						goto end_IL_0000_3;
						IL_0143:
						num2 = 23;
						DrawArrow(pStart, position);
						goto end_IL_0000_3;
						IL_012e:
						num2 = 21;
						DrawDashLine(pStart, position);
						goto end_IL_0000_3;
						end_IL_0000_2:
						break;
					}
					num2 = 49;
					rectTransparent.Rect = new Rect(num5, num7, num6 - num5, num8 - num7);
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1020;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void inkCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (ShownSubMenu != null)
		{
			CollapseSubMenus();
		}
		if (cnvLibrary.Visibility == Visibility.Visible)
		{
			PlaceLibraryImageToBack();
			e.Handled = true;
		}
		else if (DrawState != DrawStateEnum.Pen)
		{
			pStart = e.GetPosition(inkCanvas);
			isDraw = true;
			if (DrawState == DrawStateEnum.Curtain)
			{
				Mouse.Capture(inkCanvas);
			}
		}
	}

	private void inkCanvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		GestureDown(e.GetPosition(this));
	}

	private void inkCanvas_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (isGestureVisible)
		{
			GestureUp();
		}
		if (cnvLibraryBack.Visibility == Visibility.Visible)
		{
			Point position = e.GetPosition(this);
			ReselectLibrary(position);
		}
	}

	private void inkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
	{
		checked
		{
			FrameNo++;
			if (iRedo != 0)
			{
				ResetRedo();
			}
			if (FrameNo == 1)
			{
				SelectUndoImageOfFav();
			}
		}
	}

	private void inkCanvas_StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								int[] frameNoOfErase = null;
					StrokeCollection[] undoStrokeCollectionStack = null;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (WaitForEraserMouseUp)
						{
							goto end_IL_0000;
						}
						goto IL_000d;
					case 324:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000_2;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000d;
							case 3:
								goto IL_0017;
							case 4:
								goto IL_001e;
							case 5:
								goto IL_002f;
							case 6:
								goto IL_0039;
							case 7:
								goto IL_0041;
							case 8:
								goto IL_004d;
							case 9:
								goto IL_0055;
							case 10:
								goto IL_0066;
							case 11:
								goto IL_008b;
							case 12:
								goto IL_00a2;
							case 13:
								goto IL_00c9;
							case 14:
								goto end_IL_0000_3;
							default:
								goto end_IL_0000_2;
							case 15:
								goto end_IL_0000;
							}
							goto default;
						}
						IL_00c9:
						num2 = 13;
						UndoStrokeCollectionStack[iErase] = inkCanvas.Strokes.Clone();
						break;
						IL_000d:
						num2 = 2;
						if (IgnoreErrors)
						{
							goto IL_0017;
						}
						goto IL_001e;
						IL_0017:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_001e;
						IL_001e:
						num2 = 4;
						FrameNo++;
						goto IL_002f;
						IL_002f:
						num2 = 5;
						if (iRedo != 0)
						{
							goto IL_0039;
						}
						goto IL_0041;
						IL_0039:
						num2 = 6;
						ResetRedo();
						goto IL_0041;
						IL_0041:
						num2 = 7;
						if (FrameNo == 1)
						{
							goto IL_004d;
						}
						goto IL_0055;
						IL_004d:
						num2 = 8;
						SelectUndoImageOfFav();
						goto IL_0055;
						IL_0055:
						num2 = 9;
						iErase++;
						goto IL_0066;
						IL_0066:
						num2 = 10;
						frameNoOfErase = FrameNoOfErase;
						frameNoOfErase = (int[])Utils.CopyArray(frameNoOfErase, new int[iErase + 1]);
						FrameNoOfErase = frameNoOfErase;
						goto IL_008b;
						IL_008b:
						num2 = 11;
						FrameNoOfErase[iErase] = (int)FrameNo;
						goto IL_00a2;
						IL_00a2:
						num2 = 12;
						undoStrokeCollectionStack = UndoStrokeCollectionStack;
						undoStrokeCollectionStack = (StrokeCollection[])Utils.CopyArray(undoStrokeCollectionStack, new StrokeCollection[iErase + 1]);
						UndoStrokeCollectionStack = undoStrokeCollectionStack;
						goto IL_00c9;
						end_IL_0000_3:
						break;
					}
					num2 = 14;
					WaitForEraserMouseUp = true;
					break;
								end_IL_0000_2:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 324;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void inkCanvas_PreviewStylusDown(object sender, StylusDownEventArgs e)
	{
		LastStylusDeviceID = e.StylusDevice.Id;
		if (isGestureVisible)
		{
			switch (DrawState)
			{
			case DrawStateEnum.Eraser:
				inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
				break;
			case DrawStateEnum.Pen:
				inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
				break;
			}
			cnvGesture.Visibility = Visibility.Collapsed;
			isGestureVisible = false;
			GesturesStylusDeviceID = -2;
		}
		if (cnvLibrary.Visibility == Visibility.Visible)
		{
			e.Handled = true;
		}
	}

	private void inkCanvas_PreviewStylusMove(object sender, StylusEventArgs e)
	{
		if ((e.StylusDevice.Id != LastStylusDeviceID) | (LastStylusDeviceID == GesturesStylusDeviceID))
		{
			if (!isGestureVisible)
			{
				GesturesStylusDeviceID = e.StylusDevice.Id;
				GestureDown(e.GetPosition(this));
			}
			if (e.StylusDevice.Id == GesturesStylusDeviceID)
			{
				GestureMove(e.GetPosition(this));
			}
		}
	}

	private void inkCanvas_PreviewStylusUp(object sender, StylusEventArgs e)
	{
		if (isGestureVisible && ((e.StylusDevice.Id != LastStylusDeviceID) | (LastStylusDeviceID == GesturesStylusDeviceID)) && e.StylusDevice.Id == GesturesStylusDeviceID)
		{
			GestureUp();
		}
	}

	private void imgHandAndPen_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		int num5 = default(int);
		int num6 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								bool flag;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1336:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 5:
								goto IL_0027;
							case 6:
								goto IL_002f;
							case 7:
								goto IL_004c;
							case 8:
								goto IL_0080;
							case 9:
								goto IL_00d4;
							case 10:
								goto IL_00df;
							case 12:
								goto IL_00ea;
							case 13:
								goto IL_0106;
							case 14:
								goto IL_0115;
							case 11:
							case 15:
								goto IL_011e;
							case 16:
								goto IL_012d;
							case 17:
								goto IL_0146;
							case 18:
								goto IL_0155;
							case 19:
								goto IL_0164;
							case 20:
								goto IL_0173;
							case 21:
								goto IL_0182;
							case 22:
								goto IL_0191;
							case 23:
								goto IL_01a5;
							case 24:
								goto IL_01bb;
							case 25:
								goto IL_01d6;
							case 26:
								goto IL_01e0;
							case 27:
								goto IL_01eb;
							case 28:
								goto IL_01f5;
							case 29:
								goto IL_0203;
							case 31:
								goto IL_022e;
							case 32:
								goto IL_0239;
							case 33:
								goto IL_0242;
							case 34:
								goto IL_024d;
							case 35:
								goto IL_0256;
							case 36:
								goto IL_0266;
							case 37:
								goto IL_0275;
							case 38:
								goto IL_0280;
							case 39:
								goto IL_028f;
							case 40:
								goto IL_02b2;
							case 41:
								goto IL_02bd;
							case 42:
								goto IL_02c6;
							case 43:
								goto IL_02d9;
							case 44:
								goto IL_02e8;
							case 45:
								goto IL_031f;
							case 46:
								goto IL_0336;
							case 47:
								goto IL_034f;
							case 48:
								goto IL_035e;
							case 49:
								goto IL_036d;
							case 50:
								goto IL_037c;
							case 51:
								goto IL_038b;
							case 52:
								goto IL_039a;
							case 53:
								goto IL_03a9;
							case 54:
								goto IL_03b8;
							case 55:
								goto IL_03c7;
							case 56:
								goto IL_03db;
							case 57:
								goto IL_03f1;
							case 58:
								goto IL_040c;
							case 59:
								goto IL_0417;
							case 60:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 4:
							case 30:
							case 61:
							case 62:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_03db:
						num2 = 56;
						imgFav[i].Visibility = Visibility.Collapsed;
						goto IL_03f1;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						flag = isMinimized;
						if (flag)
						{
							if (!flag)
							{
								goto end_IL_0000_3;
							}
							goto IL_0027;
						}
						goto IL_022e;
						IL_03f1:
						num2 = 57;
						i++;
						goto IL_0402;
						IL_0027:
						num2 = 5;
						SetWindowSize();
						goto IL_002f;
						IL_002f:
						num2 = 6;
						borderMainMenu.Height = 223 + iFav * 42 + ByLineHeight;
						ShowByLine(isVisible: true);
						goto IL_004c;
						IL_004c:
						num2 = 7;
						if (cnvMainMenu.Margin.Top + borderMainMenu.Height + 10.0 > base.Height)
						{
							goto IL_0080;
						}
						goto IL_00d4;
						IL_0080:
						num2 = 8;
						cnvMainMenu.Margin = new Thickness(cnvMainMenu.Margin.Left, base.Height - borderMainMenu.Height - 10.0, 0.0, 0.0);
						goto IL_00d4;
						IL_00d4:
						num2 = 9;
						if (DrawState != DrawStateEnum.Pen)
						{
							goto IL_00df;
						}
						goto IL_00ea;
						IL_00df:
						num2 = 10;
						ActivatePen();
						goto IL_011e;
						IL_00ea:
						num2 = 12;
						inkCanvas.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
						goto IL_0106;
						IL_0106:
						num2 = 13;
						inkCanvas.IsEnabled = true;
						goto IL_0115;
						IL_0115:
						num2 = 14;
						ChangeTickOfMode();
						goto IL_011e;
						IL_011e:
						num2 = 15;
						inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
						goto IL_012d;
						IL_012d:
						num2 = 16;
						imgHandAndPen.Source = imgHandRes.Source;
						goto IL_0146;
						IL_0146:
						num2 = 17;
						imgPen.Visibility = Visibility.Visible;
						goto IL_0155;
						IL_0155:
						num2 = 18;
						imgEraser.Visibility = Visibility.Visible;
						goto IL_0164;
						IL_0164:
						num2 = 19;
						imgShape.Visibility = Visibility.Visible;
						goto IL_0173;
						IL_0173:
						num2 = 20;
						imgSettings.Visibility = Visibility.Visible;
						goto IL_0182;
						IL_0182:
						num2 = 21;
						imgClose.Visibility = Visibility.Visible;
						goto IL_0191;
						IL_0191:
						num2 = 22;
						num5 = iFav;
						i = 1;
						goto IL_01cc;
						IL_01cc:
						if (i <= num5)
						{
							goto IL_01a5;
						}
						goto IL_01d6;
						IL_01d6:
						num2 = 25;
						isMinimized = false;
						goto IL_01e0;
						IL_01e0:
						num2 = 26;
						if (!DebugMode)
						{
							goto IL_01eb;
						}
						goto IL_01f5;
						IL_01eb:
						num2 = 27;
						base.Topmost = true;
						goto IL_01f5;
						IL_01f5:
						num2 = 28;
						if (isFirstRun)
						{
							goto end_IL_0000_3;
						}
						goto IL_0203;
						IL_0203:
						num2 = 29;
						AnimationFadeIn(rectFrame, 200);
						goto end_IL_0000_3;
						IL_01a5:
						num2 = 23;
						imgFav[i].Visibility = Visibility.Visible;
						goto IL_01bb;
						IL_01bb:
						num2 = 24;
						i++;
						goto IL_01cc;
						IL_022e:
						num2 = 31;
						if (isTrianglesSecondStep)
						{
							goto IL_0239;
						}
						goto IL_0242;
						IL_0239:
						num2 = 32;
						CancelDrawingOfTriangle();
						goto IL_0242;
						IL_0242:
						num2 = 33;
						if (ShownSubMenu != null)
						{
							goto IL_024d;
						}
						goto IL_0256;
						IL_024d:
						num2 = 34;
						CollapseSubMenus();
						goto IL_0256;
						IL_0256:
						num2 = 35;
						if (borderSettings.Visibility == Visibility.Visible)
						{
							goto IL_0266;
						}
						goto IL_0275;
						IL_0266:
						num2 = 36;
						borderSettings.Visibility = Visibility.Collapsed;
						goto IL_0275;
						IL_0275:
						num2 = 37;
						if (isGestureVisible)
						{
							goto IL_0280;
						}
						goto IL_028f;
						IL_0280:
						num2 = 38;
						cnvGesture.Visibility = Visibility.Collapsed;
						goto IL_028f;
						IL_028f:
						num2 = 39;
						AnimationFadeOut(rectFrame, 100);
						goto IL_02b2;
						IL_02b2:
						num2 = 40;
						if (!isFirstRun)
						{
							goto IL_02bd;
						}
						goto IL_02c6;
						IL_02bd:
						num2 = 41;
						ClearCanvas();
						goto IL_02c6;
						IL_02c6:
						num2 = 42;
						inkCanvas.Background = Brushes.Transparent;
						goto IL_02d9;
						IL_02d9:
						num2 = 43;
						inkCanvas.IsEnabled = false;
						goto IL_02e8;
						IL_02e8:
						num2 = 44;
						rectTransparent.Rect = new Rect(0.0, 0.0, 0.0, 0.0);
						goto IL_031f;
						IL_031f:
						num2 = 45;
						borderMainMenu.Height = 76.0;
						ShowByLine(isVisible: false);
						goto IL_0336;
						IL_0336:
						num2 = 46;
						imgHandAndPen.Source = imgPenDrawing.Source;
						goto IL_034f;
						IL_034f:
						num2 = 47;
						imgPen.Visibility = Visibility.Collapsed;
						goto IL_035e;
						IL_035e:
						num2 = 48;
						imgEraser.Visibility = Visibility.Collapsed;
						goto IL_036d;
						IL_036d:
						num2 = 49;
						imgShape.Visibility = Visibility.Collapsed;
						goto IL_037c;
						IL_037c:
						num2 = 50;
						imgSettings.Visibility = Visibility.Collapsed;
						goto IL_038b;
						IL_038b:
						num2 = 51;
						imgClose.Visibility = Visibility.Collapsed;
						goto IL_039a;
						IL_039a:
						num2 = 52;
						imgTickOfPen.Visibility = Visibility.Collapsed;
						goto IL_03a9;
						IL_03a9:
						num2 = 53;
						imgTickOfEraser.Visibility = Visibility.Collapsed;
						goto IL_03b8;
						IL_03b8:
						num2 = 54;
						imgTickOfShape.Visibility = Visibility.Collapsed;
						goto IL_03c7;
						IL_03c7:
						num2 = 55;
						num6 = iFav;
						i = 1;
						goto IL_0402;
						IL_0402:
						if (i <= num6)
						{
							goto IL_03db;
						}
						goto IL_040c;
						IL_040c:
						num2 = 58;
						if (DebugMode)
						{
							break;
						}
						goto IL_0417;
						IL_0417:
						num2 = 59;
						base.Topmost = true;
						break;
						end_IL_0000_2:
						break;
					}
					num2 = 60;
					isMinimized = true;
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1336;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 399:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_0022;
						case 5:
							goto IL_002c;
						case 6:
							goto IL_0034;
						case 7:
							goto IL_003e;
						case 8:
							goto IL_0081;
						case 11:
							goto IL_008e;
						case 10:
						case 12:
							goto IL_0097;
						case 13:
							goto IL_00bf;
						case 14:
							goto IL_00ce;
						case 16:
							goto IL_00da;
						case 17:
							goto IL_00e9;
						case 15:
						case 18:
							goto IL_00f3;
						case 20:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 9:
						case 19:
						case 21:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00f3:
					num2 = 18;
					AnimationFadeIn(ShownSubMenu, 200);
					goto end_IL_0000_3;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (!MySettingsProperty.Settings.isCLoseConfirmationEnabled)
					{
						break;
					}
					goto IL_0022;
					IL_0022:
					num2 = 4;
					if (isTrianglesSecondStep)
					{
						goto IL_002c;
					}
					goto IL_0034;
					IL_002c:
					num2 = 5;
					CancelDrawingOfTriangle();
					goto IL_0034;
					IL_0034:
					num2 = 6;
					if (ShownSubMenu != null)
					{
						goto IL_003e;
					}
					goto IL_0097;
					IL_003e:
					num2 = 7;
					if ((Operators.CompareString(ShownSubMenu.Name, GridSubMenuCloseRight.Name, TextCompare: false) == 0) | (Operators.CompareString(ShownSubMenu.Name, GridSubMenuCloseLeft.Name, TextCompare: false) == 0))
					{
						goto IL_0081;
					}
					goto IL_008e;
					IL_0081:
					num2 = 8;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_008e:
					num2 = 11;
					CollapseSubMenus();
					goto IL_0097;
					IL_0097:
					num2 = 12;
					if (cnvMainMenu.Margin.Left + 470.0 > base.Width)
					{
						goto IL_00bf;
					}
					goto IL_00da;
					IL_00bf:
					num2 = 13;
					ShownSubMenu = GridSubMenuCloseLeft;
					goto IL_00ce;
					IL_00ce:
					num2 = 14;
					isLeftSubMenu = true;
					goto IL_00f3;
					IL_00da:
					num2 = 16;
					ShownSubMenu = GridSubMenuCloseRight;
					goto IL_00e9;
					IL_00e9:
					num2 = 17;
					isLeftSubMenu = false;
					goto IL_00f3;
					end_IL_0000_2:
					break;
				}
				num2 = 20;
				CloseMe(null, null);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 399;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgPen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 808:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001b;
						case 5:
							goto IL_0023;
						case 6:
							goto IL_002d;
						case 7:
							goto IL_0070;
						case 10:
							goto IL_007d;
						case 9:
						case 11:
							goto IL_0086;
						case 12:
							goto IL_0097;
						case 13:
							goto IL_00b3;
						case 14:
							goto IL_00bc;
						case 15:
							goto IL_00e7;
						case 16:
							goto IL_00f6;
						case 17:
							goto IL_0100;
						case 19:
							goto IL_0123;
						case 20:
							goto IL_013c;
						case 22:
							goto IL_014b;
						case 23:
							goto IL_0164;
						case 25:
							goto IL_0173;
						case 26:
							goto IL_018c;
						case 29:
							goto IL_019b;
						case 30:
							goto IL_01aa;
						case 31:
							goto IL_01b4;
						case 33:
							goto IL_01d4;
						case 34:
							goto IL_01ed;
						case 36:
							goto IL_01f9;
						case 37:
							goto IL_0212;
						case 39:
							goto IL_021e;
						case 40:
							goto IL_0237;
						case 18:
						case 21:
						case 24:
						case 27:
						case 28:
						case 32:
						case 35:
						case 38:
						case 41:
						case 42:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 8:
						case 43:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_01ed:
					num2 = 34;
					ActiveTab = PenStyleEnum.Marker;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (isTrianglesSecondStep)
					{
						goto IL_001b;
					}
					goto IL_0023;
					IL_001b:
					num2 = 4;
					CancelDrawingOfTriangle();
					goto IL_0023;
					IL_0023:
					num2 = 5;
					if (ShownSubMenu != null)
					{
						goto IL_002d;
					}
					goto IL_0086;
					IL_002d:
					num2 = 6;
					if ((Operators.CompareString(ShownSubMenu.Name, GridSubMenuPenRight.Name, TextCompare: false) == 0) | (Operators.CompareString(ShownSubMenu.Name, GridSubMenuPenLeft.Name, TextCompare: false) == 0))
					{
						goto IL_0070;
					}
					goto IL_007d;
					IL_0070:
					num2 = 7;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_007d:
					num2 = 10;
					CollapseSubMenus();
					goto IL_0086;
					IL_0086:
					num2 = 11;
					if (imgTickOfPen.Visibility == Visibility.Collapsed)
					{
						goto IL_0097;
					}
					goto IL_00bc;
					IL_0097:
					num2 = 12;
					if (Operators.ConditionalCompareObjectEqual(imgPen.Tag, DrawStateEnum.Pen, TextCompare: false))
					{
						goto IL_00b3;
					}
					goto IL_00bc;
					IL_00b3:
					num2 = 13;
					ActivatePen();
					goto IL_00bc;
					IL_00bc:
					num2 = 14;
					if (cnvMainMenu.Margin.Left + 470.0 > base.Width)
					{
						goto IL_00e7;
					}
					goto IL_019b;
					IL_00e7:
					num2 = 15;
					ShownSubMenu = GridSubMenuPenLeft;
					goto IL_00f6;
					IL_00f6:
					num2 = 16;
					isLeftSubMenu = true;
					goto IL_0100;
					IL_0100:
					num2 = 17;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_014b;
					case PenStyleEnum.Highlighter:
						goto IL_0173;
					default:
						goto end_IL_0000_2;
					}
					goto IL_0123;
					IL_0173:
					num2 = 25;
					imgSubMenuPenLeft.Source = imgSubMenuHighlighterLeft.Source;
					goto IL_018c;
					IL_018c:
					num2 = 26;
					ActiveTab = PenStyleEnum.Highlighter;
					break;
					IL_014b:
					num2 = 22;
					imgSubMenuPenLeft.Source = imgSubMenuStylographLeft.Source;
					goto IL_0164;
					IL_0164:
					num2 = 23;
					ActiveTab = PenStyleEnum.Stylograph;
					break;
					IL_0123:
					num2 = 19;
					imgSubMenuPenLeft.Source = imgSubMenuMarkerLeft.Source;
					goto IL_013c;
					IL_013c:
					num2 = 20;
					ActiveTab = PenStyleEnum.Marker;
					break;
					IL_019b:
					num2 = 29;
					ShownSubMenu = GridSubMenuPenRight;
					goto IL_01aa;
					IL_01aa:
					num2 = 30;
					isLeftSubMenu = false;
					goto IL_01b4;
					IL_01b4:
					num2 = 31;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_01f9;
					case PenStyleEnum.Highlighter:
						goto IL_021e;
					default:
						goto end_IL_0000_2;
					}
					goto IL_01d4;
					IL_021e:
					num2 = 39;
					imgSubMenuPenRight.Source = imgSubMenuHighlighterRight.Source;
					goto IL_0237;
					IL_0237:
					num2 = 40;
					ActiveTab = PenStyleEnum.Highlighter;
					break;
					IL_01f9:
					num2 = 36;
					imgSubMenuPenRight.Source = imgSubMenuStylographRight.Source;
					goto IL_0212;
					IL_0212:
					num2 = 37;
					ActiveTab = PenStyleEnum.Stylograph;
					break;
					IL_01d4:
					num2 = 33;
					imgSubMenuPenRight.Source = imgSubMenuMarkerRight.Source;
					goto IL_01ed;
					end_IL_0000_2:
					break;
				}
				num2 = 42;
				AnimationFadeIn(ShownSubMenu, 200);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 808;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgEraser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 596:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001b;
						case 5:
							goto IL_0023;
						case 6:
							goto IL_002d;
						case 7:
							goto IL_0070;
						case 10:
							goto IL_007d;
						case 9:
						case 11:
							goto IL_0086;
						case 12:
							goto IL_00ae;
						case 13:
							goto IL_00bd;
						case 14:
							goto IL_00c7;
						case 15:
							goto IL_00d2;
						case 17:
							goto IL_00e3;
						case 16:
						case 18:
							goto IL_00f2;
						case 19:
							goto IL_00fd;
						case 21:
							goto IL_0111;
						case 23:
							goto IL_0122;
						case 24:
							goto IL_0131;
						case 25:
							goto IL_013b;
						case 26:
							goto IL_0146;
						case 28:
							goto IL_0157;
						case 27:
						case 29:
							goto IL_0166;
						case 30:
							goto IL_0171;
						case 32:
							goto IL_0182;
						case 20:
						case 22:
						case 31:
						case 33:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 8:
						case 34:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0182:
					num2 = 32;
					imgGrayedUndoRight.Visibility = Visibility.Collapsed;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (isTrianglesSecondStep)
					{
						goto IL_001b;
					}
					goto IL_0023;
					IL_001b:
					num2 = 4;
					CancelDrawingOfTriangle();
					goto IL_0023;
					IL_0023:
					num2 = 5;
					if (ShownSubMenu != null)
					{
						goto IL_002d;
					}
					goto IL_0086;
					IL_002d:
					num2 = 6;
					if ((Operators.CompareString(ShownSubMenu.Name, GridSubMenuEraserRight.Name, TextCompare: false) == 0) | (Operators.CompareString(ShownSubMenu.Name, GridSubMenuEraserLeft.Name, TextCompare: false) == 0))
					{
						goto IL_0070;
					}
					goto IL_007d;
					IL_0070:
					num2 = 7;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_007d:
					num2 = 10;
					CollapseSubMenus();
					goto IL_0086;
					IL_0086:
					num2 = 11;
					if (cnvMainMenu.Margin.Left + 470.0 > base.Width)
					{
						goto IL_00ae;
					}
					goto IL_0122;
					IL_00ae:
					num2 = 12;
					ShownSubMenu = GridSubMenuEraserLeft;
					goto IL_00bd;
					IL_00bd:
					num2 = 13;
					isLeftSubMenu = true;
					goto IL_00c7;
					IL_00c7:
					num2 = 14;
					if (iRedo == 0)
					{
						goto IL_00d2;
					}
					goto IL_00e3;
					IL_00d2:
					num2 = 15;
					imgGrayedRedoLeft.Visibility = Visibility.Visible;
					goto IL_00f2;
					IL_00e3:
					num2 = 17;
					imgGrayedRedoLeft.Visibility = Visibility.Collapsed;
					goto IL_00f2;
					IL_00f2:
					num2 = 18;
					if (FrameNo == 0L)
					{
						goto IL_00fd;
					}
					goto IL_0111;
					IL_00fd:
					num2 = 19;
					imgGrayedUndoLeft.Visibility = Visibility.Visible;
					break;
					IL_0111:
					num2 = 21;
					imgGrayedUndoLeft.Visibility = Visibility.Collapsed;
					break;
					IL_0122:
					num2 = 23;
					ShownSubMenu = GridSubMenuEraserRight;
					goto IL_0131;
					IL_0131:
					num2 = 24;
					isLeftSubMenu = false;
					goto IL_013b;
					IL_013b:
					num2 = 25;
					if (iRedo == 0)
					{
						goto IL_0146;
					}
					goto IL_0157;
					IL_0146:
					num2 = 26;
					imgGrayedRedoRight.Visibility = Visibility.Visible;
					goto IL_0166;
					IL_0157:
					num2 = 28;
					imgGrayedRedoRight.Visibility = Visibility.Collapsed;
					goto IL_0166;
					IL_0166:
					num2 = 29;
					if (FrameNo == 0L)
					{
						goto IL_0171;
					}
					goto IL_0182;
					IL_0171:
					num2 = 30;
					imgGrayedUndoRight.Visibility = Visibility.Visible;
					break;
					end_IL_0000_2:
					break;
				}
				num2 = 33;
				AnimationFadeIn(ShownSubMenu, 200);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 596;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgShape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 357:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001b;
						case 5:
							goto IL_0023;
						case 6:
							goto IL_002d;
						case 7:
							goto IL_0070;
						case 10:
							goto IL_007d;
						case 9:
						case 11:
							goto IL_0086;
						case 12:
							goto IL_00ae;
						case 13:
							goto IL_00bd;
						case 15:
							goto IL_00c9;
						case 16:
							goto IL_00d8;
						case 14:
						case 17:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 8:
						case 18:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00d8:
					num2 = 16;
					isLeftSubMenu = false;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (isTrianglesSecondStep)
					{
						goto IL_001b;
					}
					goto IL_0023;
					IL_001b:
					num2 = 4;
					CancelDrawingOfTriangle();
					goto IL_0023;
					IL_0023:
					num2 = 5;
					if (ShownSubMenu != null)
					{
						goto IL_002d;
					}
					goto IL_0086;
					IL_002d:
					num2 = 6;
					if ((Operators.CompareString(ShownSubMenu.Name, GridSubMenuShapeRight.Name, TextCompare: false) == 0) | (Operators.CompareString(ShownSubMenu.Name, GridSubMenuShapeLeft.Name, TextCompare: false) == 0))
					{
						goto IL_0070;
					}
					goto IL_007d;
					IL_0070:
					num2 = 7;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_007d:
					num2 = 10;
					CollapseSubMenus();
					goto IL_0086;
					IL_0086:
					num2 = 11;
					if (cnvMainMenu.Margin.Left + 470.0 > base.Width)
					{
						goto IL_00ae;
					}
					goto IL_00c9;
					IL_00ae:
					num2 = 12;
					ShownSubMenu = GridSubMenuShapeLeft;
					goto IL_00bd;
					IL_00bd:
					num2 = 13;
					isLeftSubMenu = true;
					break;
					IL_00c9:
					num2 = 15;
					ShownSubMenu = GridSubMenuShapeRight;
					goto IL_00d8;
					end_IL_0000_2:
					break;
				}
				num2 = 17;
				AnimationFadeIn(ShownSubMenu, 200);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 357;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void Settings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		int num5 = default(int);
		int num6 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 312:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001b;
						case 5:
							goto IL_0023;
						case 6:
							goto IL_002d;
						case 7:
							goto IL_0035;
						case 8:
							goto IL_005a;
						case 9:
							goto IL_0087;
						case 10:
							goto IL_00b1;
						case 11:
							goto IL_00ba;
						case 12:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 13:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00ba:
					num2 = 11;
					GridLogo.Visibility = Visibility.Visible;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (isTrianglesSecondStep)
					{
						goto IL_001b;
					}
					goto IL_0023;
					IL_001b:
					num2 = 4;
					CancelDrawingOfTriangle();
					goto IL_0023;
					IL_0023:
					num2 = 5;
					if (ShownSubMenu != null)
					{
						goto IL_002d;
					}
					goto IL_0035;
					IL_002d:
					num2 = 6;
					CollapseSubMenus();
					goto IL_0035;
					IL_0035:
					num2 = 7;
					num5 = checked((int)Math.Round((base.Width - borderSettings.Width) / 2.0));
					goto IL_005a;
					IL_005a:
					num2 = 8;
					num6 = checked((int)Math.Round(base.Top + (base.Height - borderSettings.Height) / 2.0));
					goto IL_0087;
					IL_0087:
					num2 = 9;
					borderSettings.Margin = new Thickness(num5, num6, 0.0, 0.0);
					goto IL_00b1;
					IL_00b1:
					num2 = 10;
					ResetSettingsWindow();
					goto IL_00ba;
					end_IL_0000_2:
					break;
				}
				num2 = 12;
				AnimationFadeIn(borderSettings, 300);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 312;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgColorSelector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double x = default(double);
		Point position = default(Point);
		double y = default(double);
		double x2 = default(double);
		double x3 = default(double);
		double x4 = default(double);
		double x5 = default(double);
		double x6 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 1773:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_0020;
						case 6:
							goto IL_002b;
						case 8:
							goto IL_003d;
						case 9:
							goto IL_004f;
						case 11:
							goto IL_005b;
						case 13:
							goto IL_006e;
						case 14:
							goto IL_007e;
						case 16:
							goto IL_0095;
						case 17:
							goto IL_00a5;
						case 19:
							goto IL_00bf;
						case 20:
							goto IL_00cf;
						case 22:
							goto IL_00e5;
						case 23:
							goto IL_00f8;
						case 25:
							goto IL_010f;
						case 26:
							goto IL_0122;
						case 28:
							goto IL_012e;
						case 30:
							goto IL_0141;
						case 31:
							goto IL_0151;
						case 33:
							goto IL_016e;
						case 34:
							goto IL_017e;
						case 36:
							goto IL_0198;
						case 37:
							goto IL_01a8;
						case 39:
							goto IL_01bf;
						case 40:
							goto IL_01d2;
						case 42:
							goto IL_01e9;
						case 43:
							goto IL_01fc;
						case 45:
							goto IL_0208;
						case 47:
							goto IL_021b;
						case 48:
							goto IL_022b;
						case 50:
							goto IL_023f;
						case 51:
							goto IL_024f;
						case 53:
							goto IL_0269;
						case 54:
							goto IL_0279;
						case 56:
							goto IL_0292;
						case 57:
							goto IL_02a5;
						case 59:
							goto IL_02bf;
						case 60:
							goto IL_02d2;
						case 62:
							goto IL_02de;
						case 64:
							goto IL_02f1;
						case 65:
							goto IL_0301;
						case 67:
							goto IL_0315;
						case 68:
							goto IL_0325;
						case 70:
							goto IL_033e;
						case 71:
							goto IL_034e;
						case 73:
							goto IL_0367;
						case 74:
							goto IL_037a;
						case 76:
							goto IL_0390;
						case 77:
							goto IL_03a3;
						case 79:
							goto IL_03af;
						case 81:
							goto IL_03c2;
						case 82:
							goto IL_03d2;
						case 84:
							goto IL_03e9;
						case 85:
							goto IL_03f9;
						case 87:
							goto IL_0412;
						case 88:
							goto IL_0422;
						case 90:
							goto IL_043c;
						case 91:
							goto IL_044f;
						case 93:
							goto IL_0466;
						case 94:
							goto IL_0479;
						case 96:
							goto IL_0485;
						case 98:
							goto IL_0498;
						case 99:
							goto IL_04a8;
						case 101:
							goto IL_04bb;
						case 102:
							goto IL_04cb;
						case 104:
							goto IL_04df;
						case 105:
							goto IL_04ef;
						case 107:
							goto IL_0502;
						case 108:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
						case 7:
						case 10:
						case 12:
						case 15:
						case 18:
						case 21:
						case 24:
						case 27:
						case 29:
						case 32:
						case 35:
						case 38:
						case 41:
						case 44:
						case 46:
						case 49:
						case 52:
						case 55:
						case 58:
						case 61:
						case 63:
						case 66:
						case 69:
						case 72:
						case 75:
						case 78:
						case 80:
						case 83:
						case 86:
						case 89:
						case 92:
						case 95:
						case 97:
						case 100:
						case 103:
						case 106:
						case 109:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0502:
					num2 = 107;
					if (!(x <= 143.0))
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					position = e.GetPosition((IInputElement)sender);
					goto IL_0020;
					IL_0020:
					num2 = 4;
					y = position.Y;
					goto IL_002b;
					IL_002b:
					num2 = 6;
					if (y <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_003d;
					IL_003d:
					num2 = 8;
					if (y <= 51.0)
					{
						goto IL_004f;
					}
					goto IL_010f;
					IL_004f:
					num2 = 9;
					x2 = position.X;
					goto IL_005b;
					IL_005b:
					num2 = 11;
					if (x2 <= 23.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_006e;
					IL_006e:
					num2 = 13;
					if (x2 <= 53.0)
					{
						goto IL_007e;
					}
					goto IL_0095;
					IL_007e:
					num2 = 14;
					SelectColorFromCartela(94, 124, 139);
					goto end_IL_0000_3;
					IL_0095:
					num2 = 16;
					if (x2 <= 83.0)
					{
						goto IL_00a5;
					}
					goto IL_00bf;
					IL_00a5:
					num2 = 17;
					SelectColorFromCartela(136, 196, 64);
					goto end_IL_0000_3;
					IL_00bf:
					num2 = 19;
					if (x2 <= 113.0)
					{
						goto IL_00cf;
					}
					goto IL_00e5;
					IL_00cf:
					num2 = 20;
					SelectColorFromCartela(28, 230, 0);
					goto end_IL_0000_3;
					IL_00e5:
					num2 = 22;
					if (!(x2 <= 143.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_00f8;
					IL_00f8:
					num2 = 23;
					SelectColorFromCartela(246, 64, 44);
					goto end_IL_0000_3;
					IL_010f:
					num2 = 25;
					if (y <= 81.0)
					{
						goto IL_0122;
					}
					goto IL_01e9;
					IL_0122:
					num2 = 26;
					x3 = position.X;
					goto IL_012e;
					IL_012e:
					num2 = 28;
					if (x3 <= 23.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0141;
					IL_0141:
					num2 = 30;
					if (x3 <= 53.0)
					{
						goto IL_0151;
					}
					goto IL_016e;
					IL_0151:
					num2 = 31;
					SelectColorFromCartela(157, 157, 157);
					goto end_IL_0000_3;
					IL_016e:
					num2 = 33;
					if (x3 <= 83.0)
					{
						goto IL_017e;
					}
					goto IL_0198;
					IL_017e:
					num2 = 34;
					SelectColorFromCartela(204, 221, 30);
					goto end_IL_0000_3;
					IL_0198:
					num2 = 36;
					if (x3 <= 113.0)
					{
						goto IL_01a8;
					}
					goto IL_01bf;
					IL_01a8:
					num2 = 37;
					SelectColorFromCartela(70, 175, 74);
					goto end_IL_0000_3;
					IL_01bf:
					num2 = 39;
					if (!(x3 <= 143.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_01d2;
					IL_01d2:
					num2 = 40;
					SelectColorFromCartela(235, 20, 96);
					goto end_IL_0000_3;
					IL_01e9:
					num2 = 42;
					if (y <= 111.0)
					{
						goto IL_01fc;
					}
					goto IL_02bf;
					IL_01fc:
					num2 = 43;
					x4 = position.X;
					goto IL_0208;
					IL_0208:
					num2 = 45;
					if (x4 <= 23.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_021b;
					IL_021b:
					num2 = 47;
					if (x4 <= 53.0)
					{
						goto IL_022b;
					}
					goto IL_023f;
					IL_022b:
					num2 = 48;
					SelectColorFromCartela(88, 89, 91);
					goto end_IL_0000_3;
					IL_023f:
					num2 = 50;
					if (x4 <= 83.0)
					{
						goto IL_024f;
					}
					goto IL_0269;
					IL_024f:
					num2 = 51;
					SelectColorFromCartela(byte.MaxValue, 236, 22);
					goto end_IL_0000_3;
					IL_0269:
					num2 = 53;
					if (x4 <= 113.0)
					{
						goto IL_0279;
					}
					goto IL_0292;
					IL_0279:
					num2 = 54;
					SelectColorFromCartela(0, 150, 135);
					goto end_IL_0000_3;
					IL_0292:
					num2 = 56;
					if (!(x4 <= 143.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_02a5;
					IL_02a5:
					num2 = 57;
					SelectColorFromCartela(156, 26, 177);
					goto end_IL_0000_3;
					IL_02bf:
					num2 = 59;
					if (y <= 141.0)
					{
						goto IL_02d2;
					}
					goto IL_0390;
					IL_02d2:
					num2 = 60;
					x5 = position.X;
					goto IL_02de;
					IL_02de:
					num2 = 62;
					if (x5 <= 23.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_02f1;
					IL_02f1:
					num2 = 64;
					if (x5 <= 53.0)
					{
						goto IL_0301;
					}
					goto IL_0315;
					IL_0301:
					num2 = 65;
					SelectColorFromCartela(122, 85, 71);
					goto end_IL_0000_3;
					IL_0315:
					num2 = 67;
					if (x5 <= 83.0)
					{
						goto IL_0325;
					}
					goto IL_033e;
					IL_0325:
					num2 = 68;
					SelectColorFromCartela(byte.MaxValue, 193, 0);
					goto end_IL_0000_3;
					IL_033e:
					num2 = 70;
					if (x5 <= 113.0)
					{
						goto IL_034e;
					}
					goto IL_0367;
					IL_034e:
					num2 = 71;
					SelectColorFromCartela(0, 187, 213);
					goto end_IL_0000_3;
					IL_0367:
					num2 = 73;
					if (!(x5 <= 143.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_037a;
					IL_037a:
					num2 = 74;
					SelectColorFromCartela(96, 0, 128);
					goto end_IL_0000_3;
					IL_0390:
					num2 = 76;
					if (y <= 171.0)
					{
						goto IL_03a3;
					}
					goto IL_0466;
					IL_03a3:
					num2 = 77;
					x6 = position.X;
					goto IL_03af;
					IL_03af:
					num2 = 79;
					if (x6 <= 23.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_03c2;
					IL_03c2:
					num2 = 81;
					if (x6 <= 53.0)
					{
						goto IL_03d2;
					}
					goto IL_03e9;
					IL_03d2:
					num2 = 82;
					SelectColorFromCartela(142, 86, 46);
					goto end_IL_0000_3;
					IL_03e9:
					num2 = 84;
					if (x6 <= 83.0)
					{
						goto IL_03f9;
					}
					goto IL_0412;
					IL_03f9:
					num2 = 85;
					SelectColorFromCartela(byte.MaxValue, 152, 0);
					goto end_IL_0000_3;
					IL_0412:
					num2 = 87;
					if (x6 <= 113.0)
					{
						goto IL_0422;
					}
					goto IL_043c;
					IL_0422:
					num2 = 88;
					SelectColorFromCartela(16, 147, 245);
					goto end_IL_0000_3;
					IL_043c:
					num2 = 90;
					if (!(x6 <= 143.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_044f;
					IL_044f:
					num2 = 91;
					SelectColorFromCartela(102, 51, 185);
					goto end_IL_0000_3;
					IL_0466:
					num2 = 93;
					if (!(y <= 205.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_0479;
					IL_0479:
					num2 = 94;
					x = position.X;
					goto IL_0485;
					IL_0485:
					num2 = 96;
					if (x <= 23.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0498;
					IL_0498:
					num2 = 98;
					if (x <= 53.0)
					{
						goto IL_04a8;
					}
					goto IL_04bb;
					IL_04a8:
					num2 = 99;
					SelectColorFromCartela(byte.MaxValue, 85, 5);
					goto end_IL_0000_3;
					IL_04bb:
					num2 = 101;
					if (x <= 83.0)
					{
						goto IL_04cb;
					}
					goto IL_04df;
					IL_04cb:
					num2 = 102;
					SelectColorFromCartela(230, 27, 27);
					goto end_IL_0000_3;
					IL_04df:
					num2 = 104;
					if (x <= 113.0)
					{
						goto IL_04ef;
					}
					goto IL_0502;
					IL_04ef:
					num2 = 105;
					SelectColorFromCartela(0, 77, 230);
					goto end_IL_0000_3;
					end_IL_0000_2:
					break;
				}
				num2 = 108;
				SelectColorFromCartela(61, 77, 183);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1773;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void SubMenuEraser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, Point pFav = default(Point))
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double x = default(double);
		Point point = default(Point);
		double y = default(double);
		double x2 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 598:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_0025;
						case 6:
							goto IL_0036;
						case 5:
						case 7:
							goto IL_003a;
						case 9:
							goto IL_0045;
						case 11:
							goto IL_0058;
						case 12:
							goto IL_0068;
						case 14:
							goto IL_0074;
						case 16:
							goto IL_0087;
						case 17:
							goto IL_0097;
						case 19:
							goto IL_00a5;
						case 20:
							goto IL_00b5;
						case 22:
							goto IL_00c3;
						case 23:
							goto IL_00d6;
						case 25:
							goto IL_00e4;
						case 26:
							goto IL_00f7;
						case 28:
							goto IL_0103;
						case 30:
							goto IL_0116;
						case 31:
							goto IL_0126;
						case 33:
							goto IL_0136;
						case 34:
							goto IL_0146;
						case 36:
							goto IL_0156;
						case 37:
							goto IL_0166;
						case 39:
							goto IL_0179;
						case 40:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 8:
						case 10:
						case 13:
						case 15:
						case 18:
						case 21:
						case 24:
						case 27:
						case 29:
						case 32:
						case 35:
						case 38:
						case 41:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0179:
					num2 = 39;
					if (!(x <= 153.0))
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (pFav == default(Point))
					{
						goto IL_0025;
					}
					goto IL_0036;
					IL_0025:
					num2 = 4;
					point = e.GetPosition((IInputElement)sender);
					goto IL_003a;
					IL_0036:
					num2 = 6;
					point = pFav;
					goto IL_003a;
					IL_003a:
					num2 = 7;
					y = point.Y;
					goto IL_0045;
					IL_0045:
					num2 = 9;
					if (y <= 12.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0058;
					IL_0058:
					num2 = 11;
					if (y <= 48.0)
					{
						goto IL_0068;
					}
					goto IL_00e4;
					IL_0068:
					num2 = 12;
					x2 = point.X;
					goto IL_0074;
					IL_0074:
					num2 = 14;
					if (x2 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0087;
					IL_0087:
					num2 = 16;
					if (x2 <= 65.0)
					{
						goto IL_0097;
					}
					goto IL_00a5;
					IL_0097:
					num2 = 17;
					UndoIt();
					goto end_IL_0000_3;
					IL_00a5:
					num2 = 19;
					if (x2 <= 110.0)
					{
						goto IL_00b5;
					}
					goto IL_00c3;
					IL_00b5:
					num2 = 20;
					RedoIt();
					goto end_IL_0000_3;
					IL_00c3:
					num2 = 22;
					if (!(x2 <= 153.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_00d6;
					IL_00d6:
					num2 = 23;
					ShowCurtainSelection();
					goto end_IL_0000_3;
					IL_00e4:
					num2 = 25;
					if (!(y <= 86.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_00f7;
					IL_00f7:
					num2 = 26;
					x = point.X;
					goto IL_0103;
					IL_0103:
					num2 = 28;
					if (x <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0116;
					IL_0116:
					num2 = 30;
					if (x <= 47.0)
					{
						goto IL_0126;
					}
					goto IL_0136;
					IL_0126:
					num2 = 31;
					ActivateEraser(20, 32);
					goto end_IL_0000_3;
					IL_0136:
					num2 = 33;
					if (x <= 76.0)
					{
						goto IL_0146;
					}
					goto IL_0156;
					IL_0146:
					num2 = 34;
					ActivateEraser(50, 83);
					goto end_IL_0000_3;
					IL_0156:
					num2 = 36;
					if (x <= 111.0)
					{
						goto IL_0166;
					}
					goto IL_0179;
					IL_0166:
					num2 = 37;
					ActivateEraser(100, 160);
					goto end_IL_0000_3;
					end_IL_0000_2:
					break;
				}
				num2 = 40;
				ActivateEraser(180, 288);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 598;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void SubMenuPen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, Point pFav = default(Point))
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point point = default(Point);
		double y = default(double);
		double x = default(double);
		double x2 = default(double);
		double x3 = default(double);
		double x4 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 2847:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_0025;
						case 6:
							goto IL_0036;
						case 5:
						case 7:
							goto IL_003a;
						case 9:
							goto IL_0045;
						case 11:
							goto IL_0058;
						case 12:
							goto IL_006b;
						case 14:
							goto IL_0077;
						case 16:
							goto IL_008a;
						case 17:
							goto IL_009d;
						case 18:
							goto IL_00a8;
						case 20:
							goto IL_00c3;
						case 19:
						case 21:
							goto IL_00dc;
						case 22:
							goto IL_00ea;
						case 23:
							goto IL_00f4;
						case 24:
							goto IL_0113;
						case 25:
							goto IL_0122;
						case 26:
							goto IL_0131;
						case 28:
							goto IL_013f;
						case 29:
							goto IL_0152;
						case 30:
							goto IL_015d;
						case 32:
							goto IL_0178;
						case 31:
						case 33:
							goto IL_0191;
						case 34:
							goto IL_01a0;
						case 35:
							goto IL_01aa;
						case 36:
							goto IL_01c9;
						case 37:
							goto IL_01d8;
						case 38:
							goto IL_01e7;
						case 40:
							goto IL_01f5;
						case 41:
							goto IL_0208;
						case 42:
							goto IL_0213;
						case 44:
							goto IL_022e;
						case 43:
						case 45:
							goto IL_0247;
						case 46:
							goto IL_0256;
						case 47:
							goto IL_0260;
						case 48:
							goto IL_027f;
						case 49:
							goto IL_028e;
						case 50:
							goto IL_029d;
						case 52:
							goto IL_02ab;
						case 53:
							goto IL_02be;
						case 55:
							goto IL_02ca;
						case 57:
							goto IL_02dd;
						case 58:
							goto IL_02ed;
						case 59:
							goto IL_02fc;
						case 60:
							goto IL_0306;
						case 61:
							goto IL_030f;
						case 62:
							goto IL_031d;
						case 64:
							goto IL_032b;
						case 65:
							goto IL_033b;
						case 66:
							goto IL_034a;
						case 67:
							goto IL_0354;
						case 68:
							goto IL_035d;
						case 69:
							goto IL_036b;
						case 71:
							goto IL_0379;
						case 72:
							goto IL_0389;
						case 73:
							goto IL_0398;
						case 74:
							goto IL_03a2;
						case 75:
							goto IL_03ab;
						case 76:
							goto IL_03b9;
						case 78:
							goto IL_03c7;
						case 79:
							goto IL_03da;
						case 80:
							goto IL_03e9;
						case 81:
							goto IL_03f3;
						case 82:
							goto IL_03fc;
						case 83:
							goto IL_040a;
						case 85:
							goto IL_0418;
						case 86:
							goto IL_042b;
						case 88:
							goto IL_0437;
						case 90:
							goto IL_044a;
						case 91:
							goto IL_045a;
						case 92:
							goto IL_0469;
						case 93:
							goto IL_0473;
						case 94:
							goto IL_047c;
						case 95:
							goto IL_048a;
						case 97:
							goto IL_0498;
						case 98:
							goto IL_04a8;
						case 99:
							goto IL_04b7;
						case 100:
							goto IL_04c1;
						case 101:
							goto IL_04ca;
						case 102:
							goto IL_04d8;
						case 104:
							goto IL_04e6;
						case 105:
							goto IL_04f6;
						case 106:
							goto IL_0501;
						case 107:
							goto IL_050a;
						case 109:
							goto IL_0518;
						case 110:
							goto IL_052b;
						case 111:
							goto IL_0536;
						case 112:
							goto IL_054f;
						case 113:
							goto IL_0562;
						case 114:
							goto IL_0571;
						case 116:
							goto IL_0591;
						case 118:
							goto IL_05ac;
						case 120:
							goto IL_05c7;
						case 115:
						case 117:
						case 119:
						case 121:
						case 122:
							goto IL_05e0;
						case 123:
							goto IL_05ef;
						case 124:
							goto IL_05fd;
						case 126:
							goto IL_060b;
						case 127:
							goto IL_061e;
						case 129:
							goto IL_062a;
						case 131:
							goto IL_0640;
						case 132:
							goto IL_0653;
						case 133:
							goto IL_0660;
						case 134:
							goto IL_066c;
						case 135:
							goto IL_068f;
						case 137:
							goto IL_06a0;
						case 138:
							goto IL_06b3;
						case 139:
							goto IL_06c0;
						case 140:
							goto IL_06cc;
						case 141:
							goto IL_06ef;
						case 143:
							goto IL_0700;
						case 144:
							goto IL_0713;
						case 145:
							goto IL_0720;
						case 146:
							goto IL_072c;
						case 147:
							goto IL_074f;
						case 149:
							goto IL_0760;
						case 150:
							goto IL_0773;
						case 151:
							goto IL_0780;
						case 152:
							goto IL_078c;
						case 153:
							goto IL_07af;
						case 155:
							goto IL_07c0;
						case 156:
							goto IL_07d3;
						case 157:
							goto IL_07e0;
						case 158:
							goto IL_07ec;
						case 159:
							goto IL_080c;
						case 161:
							goto IL_081a;
						case 162:
							goto IL_082d;
						case 163:
							goto IL_083a;
						case 164:
							goto IL_0846;
						case 165:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 8:
						case 10:
						case 13:
						case 15:
						case 27:
						case 39:
						case 51:
						case 54:
						case 56:
						case 63:
						case 70:
						case 77:
						case 84:
						case 87:
						case 89:
						case 96:
						case 103:
						case 108:
						case 125:
						case 128:
						case 130:
						case 136:
						case 142:
						case 148:
						case 154:
						case 160:
						case 166:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0846:
					num2 = 164;
					if (!((ActiveTab == PenStyle) & (ShownSubMenu != null)))
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (pFav == default(Point))
					{
						goto IL_0025;
					}
					goto IL_0036;
					IL_0025:
					num2 = 4;
					point = e.GetPosition((IInputElement)sender);
					goto IL_003a;
					IL_0036:
					num2 = 6;
					point = pFav;
					goto IL_003a;
					IL_003a:
					num2 = 7;
					y = point.Y;
					goto IL_0045;
					IL_0045:
					num2 = 9;
					if (y <= 10.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0058;
					IL_0058:
					num2 = 11;
					if (y <= 32.0)
					{
						goto IL_006b;
					}
					goto IL_02ab;
					IL_006b:
					num2 = 12;
					x = point.X;
					goto IL_0077;
					IL_0077:
					num2 = 14;
					if (x <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_008a;
					IL_008a:
					num2 = 16;
					if (x <= 65.0)
					{
						goto IL_009d;
					}
					goto IL_013f;
					IL_009d:
					num2 = 17;
					if (!isLeftSubMenu)
					{
						goto IL_00a8;
					}
					goto IL_00c3;
					IL_00a8:
					num2 = 18;
					imgSubMenuPenRight.Source = imgSubMenuMarkerRight.Source;
					goto IL_00dc;
					IL_00c3:
					num2 = 20;
					imgSubMenuPenLeft.Source = imgSubMenuMarkerLeft.Source;
					goto IL_00dc;
					IL_00dc:
					num2 = 21;
					if (ActiveTab == PenStyleEnum.Marker)
					{
						goto end_IL_0000_3;
					}
					goto IL_00ea;
					IL_00ea:
					num2 = 22;
					ActiveTab = PenStyleEnum.Marker;
					goto IL_00f4;
					IL_00f4:
					num2 = 23;
					if (!Operators.ConditionalCompareObjectEqual(imgPen.Tag, DrawStateEnum.Pen, TextCompare: false))
					{
						goto end_IL_0000_3;
					}
					goto IL_0113;
					IL_0113:
					num2 = 24;
					PenStyle = ActiveTab;
					goto IL_0122;
					IL_0122:
					num2 = 25;
					ChangeColor(ColorNo);
					goto IL_0131;
					IL_0131:
					num2 = 26;
					ActivatePen();
					goto end_IL_0000_3;
					IL_013f:
					num2 = 28;
					if (x <= 106.0)
					{
						goto IL_0152;
					}
					goto IL_01f5;
					IL_0152:
					num2 = 29;
					if (!isLeftSubMenu)
					{
						goto IL_015d;
					}
					goto IL_0178;
					IL_015d:
					num2 = 30;
					imgSubMenuPenRight.Source = imgSubMenuStylographRight.Source;
					goto IL_0191;
					IL_0178:
					num2 = 32;
					imgSubMenuPenLeft.Source = imgSubMenuStylographLeft.Source;
					goto IL_0191;
					IL_0191:
					num2 = 33;
					if (ActiveTab == PenStyleEnum.Stylograph)
					{
						goto end_IL_0000_3;
					}
					goto IL_01a0;
					IL_01a0:
					num2 = 34;
					ActiveTab = PenStyleEnum.Stylograph;
					goto IL_01aa;
					IL_01aa:
					num2 = 35;
					if (!Operators.ConditionalCompareObjectEqual(imgPen.Tag, DrawStateEnum.Pen, TextCompare: false))
					{
						goto end_IL_0000_3;
					}
					goto IL_01c9;
					IL_01c9:
					num2 = 36;
					PenStyle = ActiveTab;
					goto IL_01d8;
					IL_01d8:
					num2 = 37;
					ChangeColor(ColorNo);
					goto IL_01e7;
					IL_01e7:
					num2 = 38;
					ActivatePen();
					goto end_IL_0000_3;
					IL_01f5:
					num2 = 40;
					if (!(x <= 152.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_0208;
					IL_0208:
					num2 = 41;
					if (!isLeftSubMenu)
					{
						goto IL_0213;
					}
					goto IL_022e;
					IL_0213:
					num2 = 42;
					imgSubMenuPenRight.Source = imgSubMenuHighlighterRight.Source;
					goto IL_0247;
					IL_022e:
					num2 = 44;
					imgSubMenuPenLeft.Source = imgSubMenuHighlighterLeft.Source;
					goto IL_0247;
					IL_0247:
					num2 = 45;
					if (ActiveTab == PenStyleEnum.Highlighter)
					{
						goto end_IL_0000_3;
					}
					goto IL_0256;
					IL_0256:
					num2 = 46;
					ActiveTab = PenStyleEnum.Highlighter;
					goto IL_0260;
					IL_0260:
					num2 = 47;
					if (!Operators.ConditionalCompareObjectEqual(imgPen.Tag, DrawStateEnum.Pen, TextCompare: false))
					{
						goto end_IL_0000_3;
					}
					goto IL_027f;
					IL_027f:
					num2 = 48;
					PenStyle = ActiveTab;
					goto IL_028e;
					IL_028e:
					num2 = 49;
					ChangeColor(ColorNo);
					goto IL_029d;
					IL_029d:
					num2 = 50;
					ActivatePen();
					goto end_IL_0000_3;
					IL_02ab:
					num2 = 52;
					if (y <= 69.0)
					{
						goto IL_02be;
					}
					goto IL_0418;
					IL_02be:
					num2 = 53;
					x2 = point.X;
					goto IL_02ca;
					IL_02ca:
					num2 = 55;
					if (x2 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_02dd;
					IL_02dd:
					num2 = 57;
					if (x2 <= 50.0)
					{
						goto IL_02ed;
					}
					goto IL_032b;
					IL_02ed:
					num2 = 58;
					PenStyle = ActiveTab;
					goto IL_02fc;
					IL_02fc:
					num2 = 59;
					ChangeColor(1);
					goto IL_0306;
					IL_0306:
					num2 = 60;
					ActivatePen();
					goto IL_030f;
					IL_030f:
					num2 = 61;
					if (ShownSubMenu == null)
					{
						goto end_IL_0000_3;
					}
					goto IL_031d;
					IL_031d:
					num2 = 62;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_032b:
					num2 = 64;
					if (x2 <= 84.0)
					{
						goto IL_033b;
					}
					goto IL_0379;
					IL_033b:
					num2 = 65;
					PenStyle = ActiveTab;
					goto IL_034a;
					IL_034a:
					num2 = 66;
					ChangeColor(2);
					goto IL_0354;
					IL_0354:
					num2 = 67;
					ActivatePen();
					goto IL_035d;
					IL_035d:
					num2 = 68;
					if (ShownSubMenu == null)
					{
						goto end_IL_0000_3;
					}
					goto IL_036b;
					IL_036b:
					num2 = 69;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_0379:
					num2 = 71;
					if (x2 <= 118.0)
					{
						goto IL_0389;
					}
					goto IL_03c7;
					IL_0389:
					num2 = 72;
					PenStyle = ActiveTab;
					goto IL_0398;
					IL_0398:
					num2 = 73;
					ChangeColor(3);
					goto IL_03a2;
					IL_03a2:
					num2 = 74;
					ActivatePen();
					goto IL_03ab;
					IL_03ab:
					num2 = 75;
					if (ShownSubMenu == null)
					{
						goto end_IL_0000_3;
					}
					goto IL_03b9;
					IL_03b9:
					num2 = 76;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_03c7:
					num2 = 78;
					if (!(x2 <= 152.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_03da;
					IL_03da:
					num2 = 79;
					PenStyle = ActiveTab;
					goto IL_03e9;
					IL_03e9:
					num2 = 80;
					ChangeColor(4);
					goto IL_03f3;
					IL_03f3:
					num2 = 81;
					ActivatePen();
					goto IL_03fc;
					IL_03fc:
					num2 = 82;
					if (ShownSubMenu == null)
					{
						goto end_IL_0000_3;
					}
					goto IL_040a;
					IL_040a:
					num2 = 83;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_0418:
					num2 = 85;
					if (y <= 105.0)
					{
						goto IL_042b;
					}
					goto IL_060b;
					IL_042b:
					num2 = 86;
					x3 = point.X;
					goto IL_0437;
					IL_0437:
					num2 = 88;
					if (x3 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_044a;
					IL_044a:
					num2 = 90;
					if (x3 <= 50.0)
					{
						goto IL_045a;
					}
					goto IL_0498;
					IL_045a:
					num2 = 91;
					PenStyle = ActiveTab;
					goto IL_0469;
					IL_0469:
					num2 = 92;
					ChangeColor(5);
					goto IL_0473;
					IL_0473:
					num2 = 93;
					ActivatePen();
					goto IL_047c;
					IL_047c:
					num2 = 94;
					if (ShownSubMenu == null)
					{
						goto end_IL_0000_3;
					}
					goto IL_048a;
					IL_048a:
					num2 = 95;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_0498:
					num2 = 97;
					if (x3 <= 84.0)
					{
						goto IL_04a8;
					}
					goto IL_04e6;
					IL_04a8:
					num2 = 98;
					PenStyle = ActiveTab;
					goto IL_04b7;
					IL_04b7:
					num2 = 99;
					ChangeColor(6);
					goto IL_04c1;
					IL_04c1:
					num2 = 100;
					ActivatePen();
					goto IL_04ca;
					IL_04ca:
					num2 = 101;
					if (ShownSubMenu == null)
					{
						goto end_IL_0000_3;
					}
					goto IL_04d8;
					IL_04d8:
					num2 = 102;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_04e6:
					num2 = 104;
					if (x3 <= 118.0)
					{
						goto IL_04f6;
					}
					goto IL_0518;
					IL_04f6:
					num2 = 105;
					if (ShownSubMenu != null)
					{
						goto IL_0501;
					}
					goto IL_050a;
					IL_0501:
					num2 = 106;
					CollapseSubMenus();
					goto IL_050a;
					IL_050a:
					num2 = 107;
					ShowColorSelector();
					goto end_IL_0000_3;
					IL_0518:
					num2 = 109;
					if (!(x3 <= 152.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_052b;
					IL_052b:
					num2 = 110;
					DrawState = DrawStateEnum.NoPen;
					goto IL_0536;
					IL_0536:
					num2 = 111;
					imgPen.Tag = DrawState;
					goto IL_054f;
					IL_054f:
					num2 = 112;
					inkCanvas.Background = Brushes.Transparent;
					goto IL_0562;
					IL_0562:
					num2 = 113;
					inkCanvas.IsEnabled = false;
					goto IL_0571;
					IL_0571:
					num2 = 114;
					switch (ActiveTab)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Highlighter:
						goto IL_05ac;
					case PenStyleEnum.Stylograph:
						goto IL_05c7;
					default:
						goto IL_05e0;
					}
					goto IL_0591;
					IL_05c7:
					num2 = 120;
					imgPen.Source = imgStylographNoPen.Source;
					goto IL_05e0;
					IL_05ac:
					num2 = 118;
					imgPen.Source = imgHighlighterNoPen.Source;
					goto IL_05e0;
					IL_0591:
					num2 = 116;
					imgPen.Source = imgNoPen.Source;
					goto IL_05e0;
					IL_05e0:
					num2 = 122;
					imgTickOfPen.Visibility = Visibility.Collapsed;
					goto IL_05ef;
					IL_05ef:
					num2 = 123;
					if (ShownSubMenu == null)
					{
						goto end_IL_0000_3;
					}
					goto IL_05fd;
					IL_05fd:
					num2 = 124;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_060b:
					num2 = 126;
					if (!(y <= 137.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_061e;
					IL_061e:
					num2 = 127;
					x4 = point.X;
					goto IL_062a;
					IL_062a:
					num2 = 129;
					if (x4 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0640;
					IL_0640:
					num2 = 131;
					if (x4 <= 31.0)
					{
						goto IL_0653;
					}
					goto IL_06a0;
					IL_0653:
					num2 = 132;
					ChangeInkSize(1);
					goto IL_0660;
					IL_0660:
					num2 = 133;
					ChangeInkSizeFrame();
					goto IL_066c;
					IL_066c:
					num2 = 134;
					if (!((ActiveTab == PenStyle) & (ShownSubMenu != null)))
					{
						goto end_IL_0000_3;
					}
					goto IL_068f;
					IL_068f:
					num2 = 135;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_06a0:
					num2 = 137;
					if (x4 <= 50.0)
					{
						goto IL_06b3;
					}
					goto IL_0700;
					IL_06b3:
					num2 = 138;
					ChangeInkSize(2);
					goto IL_06c0;
					IL_06c0:
					num2 = 139;
					ChangeInkSizeFrame();
					goto IL_06cc;
					IL_06cc:
					num2 = 140;
					if (!((ActiveTab == PenStyle) & (ShownSubMenu != null)))
					{
						goto end_IL_0000_3;
					}
					goto IL_06ef;
					IL_06ef:
					num2 = 141;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_0700:
					num2 = 143;
					if (x4 <= 70.0)
					{
						goto IL_0713;
					}
					goto IL_0760;
					IL_0713:
					num2 = 144;
					ChangeInkSize(3);
					goto IL_0720;
					IL_0720:
					num2 = 145;
					ChangeInkSizeFrame();
					goto IL_072c;
					IL_072c:
					num2 = 146;
					if (!((ActiveTab == PenStyle) & (ShownSubMenu != null)))
					{
						goto end_IL_0000_3;
					}
					goto IL_074f;
					IL_074f:
					num2 = 147;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_0760:
					num2 = 149;
					if (x4 <= 94.0)
					{
						goto IL_0773;
					}
					goto IL_07c0;
					IL_0773:
					num2 = 150;
					ChangeInkSize(4);
					goto IL_0780;
					IL_0780:
					num2 = 151;
					ChangeInkSizeFrame();
					goto IL_078c;
					IL_078c:
					num2 = 152;
					if (!((ActiveTab == PenStyle) & (ShownSubMenu != null)))
					{
						goto end_IL_0000_3;
					}
					goto IL_07af;
					IL_07af:
					num2 = 153;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_07c0:
					num2 = 155;
					if (x4 <= 121.0)
					{
						goto IL_07d3;
					}
					goto IL_081a;
					IL_07d3:
					num2 = 156;
					ChangeInkSize(5);
					goto IL_07e0;
					IL_07e0:
					num2 = 157;
					ChangeInkSizeFrame();
					goto IL_07ec;
					IL_07ec:
					num2 = 158;
					if (!((ActiveTab == PenStyle) & (ShownSubMenu != null)))
					{
						goto end_IL_0000_3;
					}
					goto IL_080c;
					IL_080c:
					num2 = 159;
					CollapseSubMenus();
					goto end_IL_0000_3;
					IL_081a:
					num2 = 161;
					if (!(x4 <= 151.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_082d;
					IL_082d:
					num2 = 162;
					ChangeInkSize(6);
					goto IL_083a;
					IL_083a:
					num2 = 163;
					ChangeInkSizeFrame();
					goto IL_0846;
					end_IL_0000_2:
					break;
				}
				num2 = 165;
				CollapseSubMenus();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 2847;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void SubMenuShape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, Point pFav = default(Point))
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double x = default(double);
		Point point = default(Point);
		double y = default(double);
		double x2 = default(double);
		double x3 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 765:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_002d;
						case 5:
							goto IL_0035;
						case 6:
							goto IL_005a;
						case 7:
							goto IL_006e;
						case 9:
							goto IL_007f;
						case 8:
						case 10:
							goto IL_0084;
						case 12:
							goto IL_0090;
						case 14:
							goto IL_00a3;
						case 15:
							goto IL_00b3;
						case 17:
							goto IL_00bf;
						case 19:
							goto IL_00d2;
						case 20:
							goto IL_00e2;
						case 22:
							goto IL_00f1;
						case 23:
							goto IL_0101;
						case 25:
							goto IL_0110;
						case 26:
							goto IL_0123;
						case 28:
							goto IL_0132;
						case 29:
							goto IL_0142;
						case 31:
							goto IL_014e;
						case 33:
							goto IL_0161;
						case 34:
							goto IL_0171;
						case 36:
							goto IL_0180;
						case 37:
							goto IL_0190;
						case 39:
							goto IL_019c;
						case 40:
							goto IL_01ac;
						case 42:
							goto IL_01b8;
						case 43:
							goto IL_01c8;
						case 45:
							goto IL_01d4;
						case 47:
							goto IL_01e4;
						case 48:
							goto IL_01f4;
						case 50:
							goto IL_01ff;
						case 51:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 11:
						case 13:
						case 16:
						case 18:
						case 21:
						case 24:
						case 27:
						case 30:
						case 32:
						case 35:
						case 38:
						case 41:
						case 44:
						case 46:
						case 49:
						case 52:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_01ff:
					num2 = 50;
					if (!(x <= 153.0))
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (Operators.ConditionalCompareObjectEqual(imgPen.Tag, DrawStateEnum.NoPen, TextCompare: false))
					{
						goto IL_002d;
					}
					goto IL_005a;
					IL_002d:
					num2 = 4;
					ActivatePen();
					goto IL_0035;
					IL_0035:
					num2 = 5;
					AnimationFadeIn(imgPen, 1500);
					goto IL_005a;
					IL_005a:
					num2 = 6;
					if (pFav == default(Point))
					{
						goto IL_006e;
					}
					goto IL_007f;
					IL_006e:
					num2 = 7;
					point = e.GetPosition((IInputElement)sender);
					goto IL_0084;
					IL_007f:
					num2 = 9;
					point = pFav;
					goto IL_0084;
					IL_0084:
					num2 = 10;
					y = point.Y;
					goto IL_0090;
					IL_0090:
					num2 = 12;
					if (y <= 12.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_00a3;
					IL_00a3:
					num2 = 14;
					if (y <= 43.0)
					{
						goto IL_00b3;
					}
					goto IL_0132;
					IL_00b3:
					num2 = 15;
					x2 = point.X;
					goto IL_00bf;
					IL_00bf:
					num2 = 17;
					if (x2 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_00d2;
					IL_00d2:
					num2 = 19;
					if (x2 <= 65.0)
					{
						goto IL_00e2;
					}
					goto IL_00f1;
					IL_00e2:
					num2 = 20;
					ActivateShape(DrawStateEnum.Line);
					goto end_IL_0000_3;
					IL_00f1:
					num2 = 22;
					if (x2 <= 106.0)
					{
						goto IL_0101;
					}
					goto IL_0110;
					IL_0101:
					num2 = 23;
					ActivateShape(DrawStateEnum.DashLine);
					goto end_IL_0000_3;
					IL_0110:
					num2 = 25;
					if (!(x2 <= 153.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_0123;
					IL_0123:
					num2 = 26;
					ActivateShape(DrawStateEnum.Arrow);
					goto end_IL_0000_3;
					IL_0132:
					num2 = 28;
					if (y <= 79.0)
					{
						goto IL_0142;
					}
					goto IL_01b8;
					IL_0142:
					num2 = 29;
					x3 = point.X;
					goto IL_014e;
					IL_014e:
					num2 = 31;
					if (x3 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0161;
					IL_0161:
					num2 = 33;
					if (x3 <= 65.0)
					{
						goto IL_0171;
					}
					goto IL_0180;
					IL_0171:
					num2 = 34;
					ActivateShape(DrawStateEnum.Rectangle);
					goto end_IL_0000_3;
					IL_0180:
					num2 = 36;
					if (x3 <= 106.0)
					{
						goto IL_0190;
					}
					goto IL_019c;
					IL_0190:
					num2 = 37;
					ActivateShape(DrawStateEnum.Ellipse);
					goto end_IL_0000_3;
					IL_019c:
					num2 = 39;
					if (!(x3 <= 153.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_01ac;
					IL_01ac:
					num2 = 40;
					ActivateShape(DrawStateEnum.Triangle);
					goto end_IL_0000_3;
					IL_01b8:
					num2 = 42;
					if (!(y <= 117.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_01c8;
					IL_01c8:
					num2 = 43;
					x = point.X;
					goto IL_01d4;
					IL_01d4:
					num2 = 45;
					if (x <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_01e4;
					IL_01e4:
					num2 = 47;
					if (x <= 84.0)
					{
						goto IL_01f4;
					}
					goto IL_01ff;
					IL_01f4:
					num2 = 48;
					ActivateLibrary();
					goto end_IL_0000_3;
					end_IL_0000_2:
					break;
				}
				num2 = 51;
				ActivateBackgroundPaper();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 765;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void SubMenuPen_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double x = default(double);
		Point position = default(Point);
		double y = default(double);
		double x2 = default(double);
		double x3 = default(double);
		double x4 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 2116:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_0020;
						case 6:
							goto IL_002b;
						case 8:
							goto IL_003d;
						case 9:
							goto IL_004c;
						case 11:
							goto IL_0058;
						case 13:
							goto IL_006b;
						case 15:
							goto IL_007e;
						case 17:
							goto IL_0091;
						case 19:
							goto IL_00a9;
						case 20:
							goto IL_00bc;
						case 22:
							goto IL_00c8;
						case 24:
							goto IL_00db;
						case 25:
							goto IL_00eb;
						case 27:
							goto IL_010e;
						case 29:
							goto IL_0127;
						case 31:
							goto IL_0140;
						case 34:
							goto IL_0159;
						case 35:
							goto IL_0169;
						case 37:
							goto IL_018c;
						case 39:
							goto IL_01a5;
						case 41:
							goto IL_01be;
						case 44:
							goto IL_01d7;
						case 45:
							goto IL_01e7;
						case 47:
							goto IL_020a;
						case 49:
							goto IL_0223;
						case 51:
							goto IL_023c;
						case 54:
							goto IL_0255;
						case 55:
							goto IL_0268;
						case 57:
							goto IL_028b;
						case 59:
							goto IL_02a4;
						case 61:
							goto IL_02bd;
						case 64:
							goto IL_02d6;
						case 65:
							goto IL_02e9;
						case 67:
							goto IL_02f5;
						case 69:
							goto IL_0308;
						case 70:
							goto IL_0318;
						case 72:
							goto IL_033b;
						case 74:
							goto IL_0354;
						case 76:
							goto IL_036d;
						case 79:
							goto IL_0386;
						case 80:
							goto IL_0396;
						case 82:
							goto IL_03b9;
						case 84:
							goto IL_03d2;
						case 86:
							goto IL_03eb;
						case 89:
							goto IL_0404;
						case 90:
							goto IL_0414;
						case 92:
							goto IL_0437;
						case 94:
							goto IL_0450;
						case 96:
							goto IL_0469;
						case 99:
							goto IL_0482;
						case 100:
							goto IL_0495;
						case 102:
							goto IL_04b8;
						case 104:
							goto IL_04d1;
						case 106:
							goto IL_04ea;
						case 109:
							goto IL_0503;
						case 110:
							goto IL_0516;
						case 112:
							goto IL_0522;
						case 114:
							goto IL_0535;
						case 115:
							goto IL_0545;
						case 117:
							goto IL_055e;
						case 118:
							goto IL_056e;
						case 120:
							goto IL_0587;
						case 121:
							goto IL_0597;
						case 123:
							goto IL_05ad;
						case 124:
							goto IL_05bd;
						case 126:
							goto IL_05d3;
						case 127:
							goto IL_05e3;
						case 129:
							goto IL_05f9;
						case 130:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
						case 7:
						case 10:
						case 12:
						case 14:
						case 16:
						case 18:
						case 21:
						case 23:
						case 26:
						case 28:
						case 30:
						case 32:
						case 33:
						case 36:
						case 38:
						case 40:
						case 42:
						case 43:
						case 46:
						case 48:
						case 50:
						case 52:
						case 53:
						case 56:
						case 58:
						case 60:
						case 62:
						case 63:
						case 66:
						case 68:
						case 71:
						case 73:
						case 75:
						case 77:
						case 78:
						case 81:
						case 83:
						case 85:
						case 87:
						case 88:
						case 91:
						case 93:
						case 95:
						case 97:
						case 98:
						case 101:
						case 103:
						case 105:
						case 107:
						case 108:
						case 111:
						case 113:
						case 116:
						case 119:
						case 122:
						case 125:
						case 128:
						case 131:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_05f9:
					num2 = 129;
					if (!(x <= 151.0))
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					position = e.GetPosition((IInputElement)sender);
					goto IL_0020;
					IL_0020:
					num2 = 4;
					y = position.Y;
					goto IL_002b;
					IL_002b:
					num2 = 6;
					if (y <= 10.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_003d;
					IL_003d:
					num2 = 8;
					if (y <= 32.0)
					{
						goto IL_004c;
					}
					goto IL_00a9;
					IL_004c:
					num2 = 9;
					x2 = position.X;
					goto IL_0058;
					IL_0058:
					num2 = 11;
					if (x2 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_006b;
					IL_006b:
					num2 = 13;
					if (x2 <= 65.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_007e;
					IL_007e:
					num2 = 15;
					if (x2 <= 106.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0091;
					IL_0091:
					num2 = 17;
					if (!(x2 <= 152.0))
					{
					}
					goto end_IL_0000_3;
					IL_00a9:
					num2 = 19;
					if (y <= 69.0)
					{
						goto IL_00bc;
					}
					goto IL_02d6;
					IL_00bc:
					num2 = 20;
					x3 = position.X;
					goto IL_00c8;
					IL_00c8:
					num2 = 22;
					if (x3 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_00db;
					IL_00db:
					num2 = 24;
					if (x3 <= 50.0)
					{
						goto IL_00eb;
					}
					goto IL_0159;
					IL_00eb:
					num2 = 25;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_0127;
					case PenStyleEnum.Highlighter:
						goto IL_0140;
					default:
						goto end_IL_0000_3;
					}
					goto IL_010e;
					IL_0140:
					num2 = 31;
					SetFavIcon(imgFavHighlighterRed, "HighlighterRed");
					goto end_IL_0000_3;
					IL_0127:
					num2 = 29;
					SetFavIcon(imgFavStylographRed, "StylographRed");
					goto end_IL_0000_3;
					IL_010e:
					num2 = 27;
					SetFavIcon(imgFavMarkerRed, "MarkerRed");
					goto end_IL_0000_3;
					IL_0159:
					num2 = 34;
					if (x3 <= 84.0)
					{
						goto IL_0169;
					}
					goto IL_01d7;
					IL_0169:
					num2 = 35;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_01a5;
					case PenStyleEnum.Highlighter:
						goto IL_01be;
					default:
						goto end_IL_0000_3;
					}
					goto IL_018c;
					IL_01be:
					num2 = 41;
					SetFavIcon(imgFavHighlighterBlue, "HighlighterBlue");
					goto end_IL_0000_3;
					IL_01a5:
					num2 = 39;
					SetFavIcon(imgFavStylographBlue, "StylographBlue");
					goto end_IL_0000_3;
					IL_018c:
					num2 = 37;
					SetFavIcon(imgFavMarkerBlue, "MarkerBlue");
					goto end_IL_0000_3;
					IL_01d7:
					num2 = 44;
					if (x3 <= 118.0)
					{
						goto IL_01e7;
					}
					goto IL_0255;
					IL_01e7:
					num2 = 45;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_0223;
					case PenStyleEnum.Highlighter:
						goto IL_023c;
					default:
						goto end_IL_0000_3;
					}
					goto IL_020a;
					IL_023c:
					num2 = 51;
					SetFavIcon(imgFavHighlighterGreen, "HighlighterGreen");
					goto end_IL_0000_3;
					IL_0223:
					num2 = 49;
					SetFavIcon(imgFavStylographGreen, "StylographGreen");
					goto end_IL_0000_3;
					IL_020a:
					num2 = 47;
					SetFavIcon(imgFavMarkerGreen, "MarkerGreen");
					goto end_IL_0000_3;
					IL_0255:
					num2 = 54;
					if (!(x3 <= 152.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_0268;
					IL_0268:
					num2 = 55;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_02a4;
					case PenStyleEnum.Highlighter:
						goto IL_02bd;
					default:
						goto end_IL_0000_3;
					}
					goto IL_028b;
					IL_02bd:
					num2 = 61;
					SetFavIcon(imgFavHighlighterYellow, "HighlighterYellow");
					goto end_IL_0000_3;
					IL_02a4:
					num2 = 59;
					SetFavIcon(imgFavStylographOrange, "StylographOrange");
					goto end_IL_0000_3;
					IL_028b:
					num2 = 57;
					SetFavIcon(imgFavMarkerOrange, "MarkerOrange");
					goto end_IL_0000_3;
					IL_02d6:
					num2 = 64;
					if (y <= 105.0)
					{
						goto IL_02e9;
					}
					goto IL_0503;
					IL_02e9:
					num2 = 65;
					x4 = position.X;
					goto IL_02f5;
					IL_02f5:
					num2 = 67;
					if (x4 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0308;
					IL_0308:
					num2 = 69;
					if (x4 <= 50.0)
					{
						goto IL_0318;
					}
					goto IL_0386;
					IL_0318:
					num2 = 70;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_0354;
					case PenStyleEnum.Highlighter:
						goto IL_036d;
					default:
						goto end_IL_0000_3;
					}
					goto IL_033b;
					IL_036d:
					num2 = 76;
					SetFavIcon(imgFavHighlighterBlack, "HighlighterBlack");
					goto end_IL_0000_3;
					IL_0354:
					num2 = 74;
					SetFavIcon(imgFavStylographBlack, "StylographBlack");
					goto end_IL_0000_3;
					IL_033b:
					num2 = 72;
					SetFavIcon(imgFavMarkerBlack, "MarkerBlack");
					goto end_IL_0000_3;
					IL_0386:
					num2 = 79;
					if (x4 <= 84.0)
					{
						goto IL_0396;
					}
					goto IL_0404;
					IL_0396:
					num2 = 80;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_03d2;
					case PenStyleEnum.Highlighter:
						goto IL_03eb;
					default:
						goto end_IL_0000_3;
					}
					goto IL_03b9;
					IL_03eb:
					num2 = 86;
					SetFavIcon(imgFavHighlighterWhite, "HighlighterWhite");
					goto end_IL_0000_3;
					IL_03d2:
					num2 = 84;
					SetFavIcon(imgFavStylographWhite, "StylographWhite");
					goto end_IL_0000_3;
					IL_03b9:
					num2 = 82;
					SetFavIcon(imgFavMarkerWhite, "MarkerWhite");
					goto end_IL_0000_3;
					IL_0404:
					num2 = 89;
					if (x4 <= 118.0)
					{
						goto IL_0414;
					}
					goto IL_0482;
					IL_0414:
					num2 = 90;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_0450;
					case PenStyleEnum.Highlighter:
						goto IL_0469;
					default:
						goto end_IL_0000_3;
					}
					goto IL_0437;
					IL_0469:
					num2 = 96;
					SetFavIcon(imgFavHighlighterColorful, "HighlighterColorFul");
					goto end_IL_0000_3;
					IL_0450:
					num2 = 94;
					SetFavIcon(imgFavStylographColorful, "StylographColorFul");
					goto end_IL_0000_3;
					IL_0437:
					num2 = 92;
					SetFavIcon(imgFavMarkerColorFul, "MarkerColorFul");
					goto end_IL_0000_3;
					IL_0482:
					num2 = 99;
					if (!(x4 <= 152.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_0495;
					IL_0495:
					num2 = 100;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_04d1;
					case PenStyleEnum.Highlighter:
						goto IL_04ea;
					default:
						goto end_IL_0000_3;
					}
					goto IL_04b8;
					IL_04ea:
					num2 = 106;
					SetFavIcon(imgFavHighlighterNoPen, "HighlighterNoPen");
					goto end_IL_0000_3;
					IL_04d1:
					num2 = 104;
					SetFavIcon(imgFavStylographNoPen, "StylographNoPen");
					goto end_IL_0000_3;
					IL_04b8:
					num2 = 102;
					SetFavIcon(imgFavMarkerNoPen, "MarkerNoPen");
					goto end_IL_0000_3;
					IL_0503:
					num2 = 109;
					if (!(y <= 137.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_0516;
					IL_0516:
					num2 = 110;
					x = position.X;
					goto IL_0522;
					IL_0522:
					num2 = 112;
					if (x <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0535;
					IL_0535:
					num2 = 114;
					if (x <= 31.0)
					{
						goto IL_0545;
					}
					goto IL_055e;
					IL_0545:
					num2 = 115;
					SetFavIcon(imgFavSize1, "Size1");
					goto end_IL_0000_3;
					IL_055e:
					num2 = 117;
					if (x <= 50.0)
					{
						goto IL_056e;
					}
					goto IL_0587;
					IL_056e:
					num2 = 118;
					SetFavIcon(imgFavSize2, "Size2");
					goto end_IL_0000_3;
					IL_0587:
					num2 = 120;
					if (x <= 70.0)
					{
						goto IL_0597;
					}
					goto IL_05ad;
					IL_0597:
					num2 = 121;
					SetFavIcon(imgFavSize3, "Size3");
					goto end_IL_0000_3;
					IL_05ad:
					num2 = 123;
					if (x <= 94.0)
					{
						goto IL_05bd;
					}
					goto IL_05d3;
					IL_05bd:
					num2 = 124;
					SetFavIcon(imgFavSize4, "Size4");
					goto end_IL_0000_3;
					IL_05d3:
					num2 = 126;
					if (x <= 121.0)
					{
						goto IL_05e3;
					}
					goto IL_05f9;
					IL_05e3:
					num2 = 127;
					SetFavIcon(imgFavSize5, "Size5");
					goto end_IL_0000_3;
					end_IL_0000_2:
					break;
				}
				num2 = 130;
				SetFavIcon(imgFavSize6, "Size6");
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 2116;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void SubMenuEraser_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double x = default(double);
		Point position = default(Point);
		double y = default(double);
		double x2 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 609:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_0020;
						case 6:
							goto IL_002b;
						case 8:
							goto IL_003d;
						case 9:
							goto IL_004f;
						case 11:
							goto IL_005b;
						case 13:
							goto IL_006e;
						case 14:
							goto IL_007e;
						case 16:
							goto IL_0097;
						case 17:
							goto IL_00a7;
						case 19:
							goto IL_00c0;
						case 20:
							goto IL_00d3;
						case 22:
							goto IL_00ec;
						case 23:
							goto IL_00ff;
						case 25:
							goto IL_010b;
						case 27:
							goto IL_011e;
						case 28:
							goto IL_012e;
						case 30:
							goto IL_0144;
						case 31:
							goto IL_0154;
						case 33:
							goto IL_016a;
						case 34:
							goto IL_017a;
						case 36:
							goto IL_0190;
						case 37:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 5:
						case 7:
						case 10:
						case 12:
						case 15:
						case 18:
						case 21:
						case 24:
						case 26:
						case 29:
						case 32:
						case 35:
						case 38:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0190:
					num2 = 36;
					if (!(x <= 153.0))
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					position = e.GetPosition((IInputElement)sender);
					goto IL_0020;
					IL_0020:
					num2 = 4;
					y = position.Y;
					goto IL_002b;
					IL_002b:
					num2 = 6;
					if (y <= 12.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_003d;
					IL_003d:
					num2 = 8;
					if (y <= 48.0)
					{
						goto IL_004f;
					}
					goto IL_00ec;
					IL_004f:
					num2 = 9;
					x2 = position.X;
					goto IL_005b;
					IL_005b:
					num2 = 11;
					if (x2 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_006e;
					IL_006e:
					num2 = 13;
					if (x2 <= 65.0)
					{
						goto IL_007e;
					}
					goto IL_0097;
					IL_007e:
					num2 = 14;
					SetFavIcon(imgFavUndo, "Undo");
					goto end_IL_0000_3;
					IL_0097:
					num2 = 16;
					if (x2 <= 110.0)
					{
						goto IL_00a7;
					}
					goto IL_00c0;
					IL_00a7:
					num2 = 17;
					SetFavIcon(imgFavRedo, "Redo");
					goto end_IL_0000_3;
					IL_00c0:
					num2 = 19;
					if (!(x2 <= 153.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_00d3;
					IL_00d3:
					num2 = 20;
					SetFavIcon(imgFavCurtain, "Curtain");
					goto end_IL_0000_3;
					IL_00ec:
					num2 = 22;
					if (!(y <= 86.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_00ff;
					IL_00ff:
					num2 = 23;
					x = position.X;
					goto IL_010b;
					IL_010b:
					num2 = 25;
					if (x <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_011e;
					IL_011e:
					num2 = 27;
					if (x <= 47.0)
					{
						goto IL_012e;
					}
					goto IL_0144;
					IL_012e:
					num2 = 28;
					SetFavIcon(imgFavEraser1, "Eraser1");
					goto end_IL_0000_3;
					IL_0144:
					num2 = 30;
					if (x <= 76.0)
					{
						goto IL_0154;
					}
					goto IL_016a;
					IL_0154:
					num2 = 31;
					SetFavIcon(imgFavEraser2, "Eraser2");
					goto end_IL_0000_3;
					IL_016a:
					num2 = 33;
					if (x <= 111.0)
					{
						goto IL_017a;
					}
					goto IL_0190;
					IL_017a:
					num2 = 34;
					SetFavIcon(imgFavEraser3, "Eraser3");
					goto end_IL_0000_3;
					end_IL_0000_2:
					break;
				}
				num2 = 37;
				SetFavIcon(imgFavEraser4, "Eraser4");
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 609;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void SubMenuShape_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double x = default(double);
		Point position = default(Point);
		double y = default(double);
		double x2 = default(double);
		double x3 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 819:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_002d;
						case 5:
							goto IL_0035;
						case 6:
							goto IL_005a;
						case 7:
							goto IL_0069;
						case 9:
							goto IL_0074;
						case 11:
							goto IL_0087;
						case 12:
							goto IL_009a;
						case 14:
							goto IL_00a6;
						case 16:
							goto IL_00b9;
						case 17:
							goto IL_00c9;
						case 19:
							goto IL_00e2;
						case 20:
							goto IL_00f2;
						case 22:
							goto IL_010b;
						case 23:
							goto IL_011e;
						case 25:
							goto IL_0137;
						case 26:
							goto IL_014a;
						case 28:
							goto IL_0156;
						case 30:
							goto IL_0169;
						case 31:
							goto IL_0179;
						case 33:
							goto IL_0192;
						case 34:
							goto IL_01a2;
						case 36:
							goto IL_01bb;
						case 37:
							goto IL_01ce;
						case 39:
							goto IL_01e4;
						case 40:
							goto IL_01f4;
						case 42:
							goto IL_0200;
						case 44:
							goto IL_0210;
						case 45:
							goto IL_0220;
						case 47:
							goto IL_0236;
						case 48:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 8:
						case 10:
						case 13:
						case 15:
						case 18:
						case 21:
						case 24:
						case 27:
						case 29:
						case 32:
						case 35:
						case 38:
						case 41:
						case 43:
						case 46:
						case 49:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0236:
					num2 = 47;
					if (!(x <= 153.0))
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (Operators.ConditionalCompareObjectEqual(imgPen.Tag, DrawStateEnum.NoPen, TextCompare: false))
					{
						goto IL_002d;
					}
					goto IL_005a;
					IL_002d:
					num2 = 4;
					ActivatePen();
					goto IL_0035;
					IL_0035:
					num2 = 5;
					AnimationFadeIn(imgPen, 1500);
					goto IL_005a;
					IL_005a:
					num2 = 6;
					position = e.GetPosition((IInputElement)sender);
					goto IL_0069;
					IL_0069:
					num2 = 7;
					y = position.Y;
					goto IL_0074;
					IL_0074:
					num2 = 9;
					if (y <= 12.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0087;
					IL_0087:
					num2 = 11;
					if (y <= 43.0)
					{
						goto IL_009a;
					}
					goto IL_0137;
					IL_009a:
					num2 = 12;
					x2 = position.X;
					goto IL_00a6;
					IL_00a6:
					num2 = 14;
					if (x2 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_00b9;
					IL_00b9:
					num2 = 16;
					if (x2 <= 65.0)
					{
						goto IL_00c9;
					}
					goto IL_00e2;
					IL_00c9:
					num2 = 17;
					SetFavIcon(imgFavLine, "Line");
					goto end_IL_0000_3;
					IL_00e2:
					num2 = 19;
					if (x2 <= 106.0)
					{
						goto IL_00f2;
					}
					goto IL_010b;
					IL_00f2:
					num2 = 20;
					SetFavIcon(imgFavDashLine, "DashLine");
					goto end_IL_0000_3;
					IL_010b:
					num2 = 22;
					if (!(x2 <= 153.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_011e;
					IL_011e:
					num2 = 23;
					SetFavIcon(imgFavArrow, "Arrow");
					goto end_IL_0000_3;
					IL_0137:
					num2 = 25;
					if (y <= 79.0)
					{
						goto IL_014a;
					}
					goto IL_01e4;
					IL_014a:
					num2 = 26;
					x3 = position.X;
					goto IL_0156;
					IL_0156:
					num2 = 28;
					if (x3 <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0169;
					IL_0169:
					num2 = 30;
					if (x3 <= 65.0)
					{
						goto IL_0179;
					}
					goto IL_0192;
					IL_0179:
					num2 = 31;
					SetFavIcon(imgFavRectangle, "Rectangle");
					goto end_IL_0000_3;
					IL_0192:
					num2 = 33;
					if (x3 <= 106.0)
					{
						goto IL_01a2;
					}
					goto IL_01bb;
					IL_01a2:
					num2 = 34;
					SetFavIcon(imgFavEllipse, "Ellipse");
					goto end_IL_0000_3;
					IL_01bb:
					num2 = 36;
					if (!(x3 <= 153.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_01ce;
					IL_01ce:
					num2 = 37;
					SetFavIcon(imgFavTriangle, "Triangle");
					goto end_IL_0000_3;
					IL_01e4:
					num2 = 39;
					if (!(y <= 117.0))
					{
						goto end_IL_0000_3;
					}
					goto IL_01f4;
					IL_01f4:
					num2 = 40;
					x = position.X;
					goto IL_0200;
					IL_0200:
					num2 = 42;
					if (x <= 16.0)
					{
						goto end_IL_0000_3;
					}
					goto IL_0210;
					IL_0210:
					num2 = 44;
					if (x <= 84.0)
					{
						goto IL_0220;
					}
					goto IL_0236;
					IL_0220:
					num2 = 45;
					SetFavIcon(imgFavLibrary, "Library");
					goto end_IL_0000_3;
					end_IL_0000_2:
					break;
				}
				num2 = 48;
				SetFavIcon(imgFavBackground, "Background");
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 819;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void DrawArrow(Point ArrowStart, Point ArrowEnd)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		int num5 = default(int);
		int num6 = default(int);
		int num7 = default(int);
		int num8 = default(int);
		int num9 = default(int);
		int num10 = default(int);
		int num11 = default(int);
		int num12 = default(int);
		int num13 = default(int);
		int num14 = default(int);
		int num15 = default(int);
		int num16 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1493:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_001e;
							case 5:
								goto IL_003e;
							case 6:
								goto IL_005e;
							case 8:
								goto IL_0093;
							case 9:
								goto IL_0098;
							case 11:
								goto IL_00a1;
							case 12:
								goto IL_00a7;
							case 14:
								goto IL_00b0;
							case 15:
								goto IL_00b6;
							case 17:
								goto IL_00bf;
							case 18:
								goto IL_00c5;
							case 20:
								goto IL_00ce;
							case 21:
								goto IL_00d4;
							case 23:
								goto IL_00dd;
							case 24:
								goto IL_00e3;
							case 7:
							case 10:
							case 13:
							case 16:
							case 19:
							case 22:
							case 25:
							case 26:
								goto IL_00ea;
							case 27:
								goto IL_0100;
							case 28:
								goto IL_0139;
							case 29:
								goto IL_0164;
							case 30:
								goto IL_018f;
							case 31:
								goto IL_01a8;
							case 32:
								goto IL_01c9;
							case 33:
								goto IL_01f4;
							case 34:
								goto IL_021f;
							case 35:
								goto IL_0238;
							case 37:
								goto IL_025e;
							case 38:
								goto IL_0297;
							case 39:
								goto IL_02c2;
							case 40:
								goto IL_02ed;
							case 41:
								goto IL_0306;
							case 42:
								goto IL_0327;
							case 43:
								goto IL_0352;
							case 44:
								goto IL_037d;
							case 45:
								goto IL_0396;
							case 36:
							case 46:
								goto IL_03b7;
							case 47:
								goto IL_03c2;
							case 48:
								goto IL_03dc;
							case 49:
								goto IL_03ee;
							case 50:
								goto IL_0402;
							case 51:
								goto IL_041b;
							case 52:
								goto IL_0435;
							case 53:
								goto IL_044f;
							case 54:
								goto IL_0463;
							case 55:
								goto IL_0477;
							case 56:
								goto IL_0490;
							case 57:
								goto IL_04a2;
							case 58:
								goto IL_04ad;
							case 59:
								goto IL_04b6;
							case 60:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 61:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_04b6:
						num2 = 59;
						if (FrameNo != 1)
						{
							goto end_IL_0000_3;
						}
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						pts = new StylusPointCollection();
						goto IL_001e;
						IL_001e:
						num2 = 4;
						pts.Add(new StylusPoint(ArrowStart.X, ArrowStart.Y));
						goto IL_003e;
						IL_003e:
						num2 = 5;
						pts.Add(new StylusPoint(ArrowEnd.X, ArrowEnd.Y));
						goto IL_005e;
						IL_005e:
						num2 = 6;
						switch (ShapeSize)
						{
						case 1:
							break;
						case 2:
							goto IL_00a1;
						case 3:
							goto IL_00b0;
						case 5:
							goto IL_00bf;
						case 10:
							goto IL_00ce;
						case 22:
							goto IL_00dd;
						default:
							goto IL_00ea;
						}
						goto IL_0093;
						IL_00dd:
						num2 = 23;
						num5 = 35;
						goto IL_00e3;
						IL_00e3:
						num2 = 24;
						num6 = 70;
						goto IL_00ea;
						IL_00ce:
						num2 = 20;
						num5 = 30;
						goto IL_00d4;
						IL_00d4:
						num2 = 21;
						num6 = 40;
						goto IL_00ea;
						IL_00bf:
						num2 = 17;
						num5 = 25;
						goto IL_00c5;
						IL_00c5:
						num2 = 18;
						num6 = 30;
						goto IL_00ea;
						IL_00b0:
						num2 = 14;
						num5 = 24;
						goto IL_00b6;
						IL_00b6:
						num2 = 15;
						num6 = 20;
						goto IL_00ea;
						IL_00a1:
						num2 = 11;
						num5 = 23;
						goto IL_00a7;
						IL_00a7:
						num2 = 12;
						num6 = 17;
						goto IL_00ea;
						IL_0093:
						num2 = 8;
						num5 = 25;
						goto IL_0098;
						IL_0098:
						num2 = 9;
						num6 = 14;
						goto IL_00ea;
						IL_00ea:
						num2 = 26;
						if (ArrowEnd.X < ArrowStart.X)
						{
							goto IL_0100;
						}
						goto IL_025e;
						IL_0100:
						num2 = 27;
						num7 = (int)Math.Round(Math.Atan((ArrowEnd.Y - ArrowStart.Y) / (ArrowEnd.X - ArrowStart.X)) / (Math.PI / 180.0));
						goto IL_0139;
						IL_0139:
						num2 = 28;
						num8 = (int)Math.Round(ArrowEnd.X + (double)num6 * Math.Cos((double)(num5 - num7) * (Math.PI / 180.0)));
						goto IL_0164;
						IL_0164:
						num2 = 29;
						num9 = (int)Math.Round(ArrowEnd.Y - (double)num6 * Math.Sin((double)(num5 - num7) * (Math.PI / 180.0)));
						goto IL_018f;
						IL_018f:
						num2 = 30;
						pts.Add(new StylusPoint(num8, num9));
						goto IL_01a8;
						IL_01a8:
						num2 = 31;
						pts.Add(new StylusPoint(ArrowEnd.X, ArrowEnd.Y));
						goto IL_01c9;
						IL_01c9:
						num2 = 32;
						num10 = (int)Math.Round(ArrowEnd.X + (double)num6 * Math.Cos((double)(num5 + num7) * (Math.PI / 180.0)));
						goto IL_01f4;
						IL_01f4:
						num2 = 33;
						num11 = (int)Math.Round(ArrowEnd.Y + (double)num6 * Math.Sin((double)(num5 + num7) * (Math.PI / 180.0)));
						goto IL_021f;
						IL_021f:
						num2 = 34;
						pts.Add(new StylusPoint(num10, num11));
						goto IL_0238;
						IL_0238:
						num2 = 35;
						pts.Add(new StylusPoint(ArrowEnd.X, ArrowEnd.Y));
						goto IL_03b7;
						IL_025e:
						num2 = 37;
						num12 = (int)Math.Round(Math.Atan((ArrowEnd.Y - ArrowStart.Y) / (ArrowEnd.X - ArrowStart.X)) / (Math.PI / 180.0));
						goto IL_0297;
						IL_0297:
						num2 = 38;
						num13 = (int)Math.Round(ArrowEnd.X - (double)num6 * Math.Cos((double)(num5 - num12) * (Math.PI / 180.0)));
						goto IL_02c2;
						IL_02c2:
						num2 = 39;
						num14 = (int)Math.Round(ArrowEnd.Y + (double)num6 * Math.Sin((double)(num5 - num12) * (Math.PI / 180.0)));
						goto IL_02ed;
						IL_02ed:
						num2 = 40;
						pts.Add(new StylusPoint(num13, num14));
						goto IL_0306;
						IL_0306:
						num2 = 41;
						pts.Add(new StylusPoint(ArrowEnd.X, ArrowEnd.Y));
						goto IL_0327;
						IL_0327:
						num2 = 42;
						num15 = (int)Math.Round(ArrowEnd.X - (double)num6 * Math.Cos((double)(num5 + num12) * (Math.PI / 180.0)));
						goto IL_0352;
						IL_0352:
						num2 = 43;
						num16 = (int)Math.Round(ArrowEnd.Y - (double)num6 * Math.Sin((double)(num5 + num12) * (Math.PI / 180.0)));
						goto IL_037d;
						IL_037d:
						num2 = 44;
						pts.Add(new StylusPoint(num15, num16));
						goto IL_0396;
						IL_0396:
						num2 = 45;
						pts.Add(new StylusPoint(ArrowEnd.X, ArrowEnd.Y));
						goto IL_03b7;
						IL_03b7:
						num2 = 46;
						if (st != null)
						{
							goto IL_03c2;
						}
						goto IL_03ee;
						IL_03c2:
						num2 = 47;
						inkCanvas.Strokes.Remove(st);
						goto IL_03dc;
						IL_03dc:
						num2 = 48;
						FrameNo--;
						goto IL_03ee;
						IL_03ee:
						num2 = 49;
						st = new Stroke(pts);
						goto IL_0402;
						IL_0402:
						num2 = 50;
						st.DrawingAttributes.Color = InkColor;
						goto IL_041b;
						IL_041b:
						num2 = 51;
						st.DrawingAttributes.Width = unchecked((int)ShapeSize);
						goto IL_0435;
						IL_0435:
						num2 = 52;
						st.DrawingAttributes.Height = unchecked((int)ShapeSize);
						goto IL_044f;
						IL_044f:
						num2 = 53;
						st.DrawingAttributes.StylusTip = StylusTip.Ellipse;
						goto IL_0463;
						IL_0463:
						num2 = 54;
						st.DrawingAttributes.FitToCurve = false;
						goto IL_0477;
						IL_0477:
						num2 = 55;
						inkCanvas.Strokes.Add(st);
						goto IL_0490;
						IL_0490:
						num2 = 56;
						FrameNo++;
						goto IL_04a2;
						IL_04a2:
						num2 = 57;
						if (iRedo != 0)
						{
							goto IL_04ad;
						}
						goto IL_04b6;
						IL_04ad:
						num2 = 58;
						ResetRedo();
						goto IL_04b6;
						end_IL_0000_2:
						break;
					}
					num2 = 60;
					SelectUndoImageOfFav();
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1493;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void DrawDashLine(Point DashLineStart, Point DashLineEnd)
	{
		int try0000_dispatch = -1;
		checked
		{
			int num = default(int);
			int num2 = default(int);
			int num3 = default(int);
			int num5 = default(int);
			int num6 = default(int);
			int num7 = default(int);
			int num8 = default(int);
			int num9 = default(int);
			int num10 = default(int);
			int num11 = default(int);
			int num12 = default(int);
			int num13 = default(int);
			int num14 = default(int);
			int num15 = default(int);
			int num16 = default(int);
			int num17 = default(int);
			int num18 = default(int);
			int num19 = default(int);
			while (true)
			{
				try
				{
					/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
					switch (try0000_dispatch)
					{
					case 2018:
						goto IL_07e2;
					}
					goto IL_0000_2;
					IL_07e2:
					num = num2;
					switch (num3)
					{
					case 1:
						break;
					default:
						goto end_IL_0000;
					}
					int num4 = unchecked(num + 1);
					num = 0;
					switch (num4)
					{
					case 1:
						break;
					case 2:
						goto IL_000a;
					case 3:
						goto IL_0011;
					case 4:
						goto IL_001c;
					case 5:
						goto IL_002a;
					case 6:
						goto IL_003d;
					case 7:
						goto IL_005f;
					case 8:
						goto IL_0070;
					case 9:
						goto IL_008a;
					case 10:
						goto IL_0099;
					case 12:
						goto IL_00cf;
					case 14:
						goto IL_00d7;
					case 16:
						goto IL_00e0;
					case 18:
						goto IL_00e9;
					case 20:
						goto IL_00f2;
					case 22:
						goto IL_00fb;
					case 11:
					case 13:
					case 15:
					case 17:
					case 19:
					case 21:
					case 23:
					case 24:
						goto IL_0102;
					case 25:
						goto IL_013b;
					case 26:
						goto IL_0186;
					case 27:
						goto IL_019c;
					case 28:
						goto IL_01a6;
					case 29:
						goto IL_01ce;
					case 30:
						goto IL_01fe;
					case 31:
						goto IL_0230;
					case 32:
						goto IL_0241;
					case 33:
						goto IL_0271;
					case 34:
						goto IL_02a3;
					case 35:
						goto IL_02b4;
					case 36:
						goto IL_02c2;
					case 37:
						goto IL_02db;
					case 38:
						goto IL_02f4;
					case 39:
						goto IL_0300;
					case 40:
						goto IL_0322;
					case 41:
						goto IL_0338;
					case 42:
						goto IL_0353;
					case 43:
						goto IL_036f;
					case 44:
						goto IL_038b;
					case 45:
						goto IL_03a1;
					case 46:
						goto IL_03bc;
					case 47:
						goto IL_03ce;
					case 48:
						goto IL_03d9;
					case 49:
						goto IL_03e2;
					case 50:
						goto IL_03ef;
					case 51:
						goto IL_03f8;
					case 53:
						goto IL_041b;
					case 54:
						goto IL_0425;
					case 55:
						goto IL_044d;
					case 56:
						goto IL_047f;
					case 57:
						goto IL_04af;
					case 58:
						goto IL_04c0;
					case 59:
						goto IL_04f2;
					case 60:
						goto IL_0522;
					case 61:
						goto IL_0533;
					case 62:
						goto IL_0541;
					case 63:
						goto IL_055a;
					case 64:
						goto IL_0573;
					case 65:
						goto IL_057f;
					case 66:
						goto IL_05a1;
					case 67:
						goto IL_05b7;
					case 68:
						goto IL_05d2;
					case 69:
						goto IL_05ee;
					case 70:
						goto IL_060a;
					case 71:
						goto IL_0620;
					case 72:
						goto IL_063b;
					case 73:
						goto IL_064d;
					case 74:
						goto IL_0658;
					case 75:
						goto IL_0661;
					case 76:
						goto IL_066e;
					case 77:
						goto IL_0677;
					default:
						goto end_IL_0000;
					case 52:
					case 78:
						goto end_IL_0000_2;
					}
					goto IL_0000_2;
					IL_03f8:
					num2 = 51;
					i++;
					goto IL_0409;
					IL_0000_2:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					num5 = strokeDash.Length;
					goto IL_001c;
					IL_001c:
					num2 = 4;
					num6 = num5;
					i = 1;
					goto IL_0080;
					IL_0080:
					if (i <= num6)
					{
						goto IL_002a;
					}
					goto IL_008a;
					IL_002a:
					num2 = 5;
					if (strokeDash[i - 1] != null)
					{
						goto IL_003d;
					}
					goto IL_0070;
					IL_003d:
					num2 = 6;
					inkCanvas.Strokes.Remove(strokeDash[i - 1]);
					goto IL_005f;
					IL_005f:
					num2 = 7;
					FrameNo--;
					goto IL_0070;
					IL_0070:
					num2 = 8;
					i++;
					goto IL_0080;
					IL_008a:
					num2 = 9;
					strokeDash = new Stroke[1];
					goto IL_0099;
					IL_0099:
					num2 = 10;
					switch (ShapeSize)
					{
					case 1:
						break;
					case 2:
						goto IL_00d7;
					case 3:
						goto IL_00e0;
					case 5:
						goto IL_00e9;
					case 10:
						goto IL_00f2;
					case 22:
						goto IL_00fb;
					default:
						goto IL_0102;
					}
					goto IL_00cf;
					IL_00cf:
					num2 = 12;
					num7 = 5;
					goto IL_0102;
					IL_00d7:
					num2 = 14;
					num7 = 10;
					goto IL_0102;
					IL_00e0:
					num2 = 16;
					num7 = 15;
					goto IL_0102;
					IL_00e9:
					num2 = 18;
					num7 = 20;
					goto IL_0102;
					IL_00f2:
					num2 = 20;
					num7 = 25;
					goto IL_0102;
					IL_00fb:
					num2 = 22;
					num7 = 35;
					goto IL_0102;
					IL_0102:
					num2 = 24;
					num8 = (int)Math.Round(Math.Atan((DashLineStart.Y - DashLineEnd.Y) / (DashLineEnd.X - DashLineStart.X)) / (Math.PI / 180.0));
					goto IL_013b;
					IL_013b:
					num2 = 25;
					num9 = (int)Math.Round(Math.Sqrt(Math.Pow(DashLineEnd.X - DashLineStart.X, 2.0) + Math.Pow(DashLineEnd.Y - DashLineStart.Y, 2.0)));
					goto IL_0186;
					IL_0186:
					num2 = 26;
					if (DashLineEnd.X >= DashLineStart.X)
					{
						goto IL_019c;
					}
					goto IL_041b;
					IL_041b:
					num2 = 53;
					j = 0;
					goto IL_0425;
					IL_0425:
					num2 = 54;
					num10 = (int)Math.Round((double)num9 / (double)num7 / 2.0);
					i = 1;
					goto IL_0688;
					IL_0688:
					if (i > num10)
					{
						break;
					}
					goto IL_044d;
					IL_044d:
					num2 = 55;
					num11 = (int)Math.Round((double)(-num7 * j) * Math.Cos((double)num8 * (Math.PI / 180.0)) + DashLineStart.X);
					goto IL_047f;
					IL_047f:
					num2 = 56;
					num12 = (int)Math.Round((double)(num7 * j) * Math.Sin((double)num8 * (Math.PI / 180.0)) + DashLineStart.Y);
					goto IL_04af;
					IL_04af:
					num2 = 57;
					j++;
					goto IL_04c0;
					IL_04c0:
					num2 = 58;
					num13 = (int)Math.Round((double)(-num7 * j) * Math.Cos((double)num8 * (Math.PI / 180.0)) + DashLineStart.X);
					goto IL_04f2;
					IL_04f2:
					num2 = 59;
					num14 = (int)Math.Round((double)(num7 * j) * Math.Sin((double)num8 * (Math.PI / 180.0)) + DashLineStart.Y);
					goto IL_0522;
					IL_0522:
					num2 = 60;
					j++;
					goto IL_0533;
					IL_0533:
					num2 = 61;
					pts = new StylusPointCollection();
					goto IL_0541;
					IL_0541:
					num2 = 62;
					pts.Add(new StylusPoint(num11, num12));
					goto IL_055a;
					IL_055a:
					num2 = 63;
					pts.Add(new StylusPoint(num13, num14));
					goto IL_0573;
					IL_0573:
					num2 = 64;
					num5 = strokeDash.Length;
					goto IL_057f;
					IL_057f:
					num2 = 65;
					strokeDash = (Stroke[])Utils.CopyArray(strokeDash, new Stroke[num5 + 1]);
					goto IL_05a1;
					IL_05a1:
					num2 = 66;
					strokeDash[num5] = new Stroke(pts);
					goto IL_05b7;
					IL_05b7:
					num2 = 67;
					strokeDash[num5].DrawingAttributes.Color = InkColor;
					goto IL_05d2;
					IL_05d2:
					num2 = 68;
					strokeDash[num5].DrawingAttributes.Width = unchecked((int)ShapeSize);
					goto IL_05ee;
					IL_05ee:
					num2 = 69;
					strokeDash[num5].DrawingAttributes.Height = unchecked((int)ShapeSize);
					goto IL_060a;
					IL_060a:
					num2 = 70;
					strokeDash[num5].DrawingAttributes.StylusTip = StylusTip.Ellipse;
					goto IL_0620;
					IL_0620:
					num2 = 71;
					inkCanvas.Strokes.Add(strokeDash[num5]);
					goto IL_063b;
					IL_063b:
					num2 = 72;
					FrameNo++;
					goto IL_064d;
					IL_064d:
					num2 = 73;
					if (iRedo != 0)
					{
						goto IL_0658;
					}
					goto IL_0661;
					IL_0658:
					num2 = 74;
					ResetRedo();
					goto IL_0661;
					IL_0661:
					num2 = 75;
					if (FrameNo == 1)
					{
						goto IL_066e;
					}
					goto IL_0677;
					IL_066e:
					num2 = 76;
					SelectUndoImageOfFav();
					goto IL_0677;
					IL_0677:
					num2 = 77;
					i++;
					goto IL_0688;
					IL_019c:
					num2 = 27;
					j = 0;
					goto IL_01a6;
					IL_01a6:
					num2 = 28;
					num15 = (int)Math.Round((double)num9 / (double)num7 / 2.0);
					i = 1;
					goto IL_0409;
					IL_0409:
					if (i > num15)
					{
						break;
					}
					goto IL_01ce;
					IL_01ce:
					num2 = 29;
					num16 = (int)Math.Round((double)(num7 * j) * Math.Cos((double)num8 * (Math.PI / 180.0)) + DashLineStart.X);
					goto IL_01fe;
					IL_01fe:
					num2 = 30;
					num17 = (int)Math.Round((double)(-num7 * j) * Math.Sin((double)num8 * (Math.PI / 180.0)) + DashLineStart.Y);
					goto IL_0230;
					IL_0230:
					num2 = 31;
					j++;
					goto IL_0241;
					IL_0241:
					num2 = 32;
					num18 = (int)Math.Round((double)(num7 * j) * Math.Cos((double)num8 * (Math.PI / 180.0)) + DashLineStart.X);
					goto IL_0271;
					IL_0271:
					num2 = 33;
					num19 = (int)Math.Round((double)(-num7 * j) * Math.Sin((double)num8 * (Math.PI / 180.0)) + DashLineStart.Y);
					goto IL_02a3;
					IL_02a3:
					num2 = 34;
					j++;
					goto IL_02b4;
					IL_02b4:
					num2 = 35;
					pts = new StylusPointCollection();
					goto IL_02c2;
					IL_02c2:
					num2 = 36;
					pts.Add(new StylusPoint(num16, num17));
					goto IL_02db;
					IL_02db:
					num2 = 37;
					pts.Add(new StylusPoint(num18, num19));
					goto IL_02f4;
					IL_02f4:
					num2 = 38;
					num5 = strokeDash.Length;
					goto IL_0300;
					IL_0300:
					num2 = 39;
					strokeDash = (Stroke[])Utils.CopyArray(strokeDash, new Stroke[num5 + 1]);
					goto IL_0322;
					IL_0322:
					num2 = 40;
					strokeDash[num5] = new Stroke(pts);
					goto IL_0338;
					IL_0338:
					num2 = 41;
					strokeDash[num5].DrawingAttributes.Color = InkColor;
					goto IL_0353;
					IL_0353:
					num2 = 42;
					strokeDash[num5].DrawingAttributes.Width = unchecked((int)ShapeSize);
					goto IL_036f;
					IL_036f:
					num2 = 43;
					strokeDash[num5].DrawingAttributes.Height = unchecked((int)ShapeSize);
					goto IL_038b;
					IL_038b:
					num2 = 44;
					strokeDash[num5].DrawingAttributes.StylusTip = StylusTip.Ellipse;
					goto IL_03a1;
					IL_03a1:
					num2 = 45;
					inkCanvas.Strokes.Add(strokeDash[num5]);
					goto IL_03bc;
					IL_03bc:
					num2 = 46;
					FrameNo++;
					goto IL_03ce;
					IL_03ce:
					num2 = 47;
					if (iRedo != 0)
					{
						goto IL_03d9;
					}
					goto IL_03e2;
					IL_03d9:
					num2 = 48;
					ResetRedo();
					goto IL_03e2;
					IL_03e2:
					num2 = 49;
					if (FrameNo == 1)
					{
						goto IL_03ef;
					}
					goto IL_03f8;
					IL_03ef:
					num2 = 50;
					SelectUndoImageOfFav();
					goto IL_03f8;
					end_IL_0000:;
				}
				catch (Exception) when (unchecked(num3 != 0 && num == 0))
				{
					// Error handled by On Error Resume Next
					try0000_dispatch = 2018;
					continue;
				}
				throw ProjectData.CreateProjectError(-2146828237);
				continue;
				end_IL_0000_2:
				break;
			}
			if (num != 0)
			{
				ProjectData.ClearProjectError();
			}
		}
	}

	private void DrawEllipse(Point EllipseStart, Point ELlipseEnd)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		int num5 = default(int);
		double num6 = default(double);
		double num7 = default(double);
		double num8 = default(double);
		double num9 = default(double);
		double num10 = default(double);
		int num11 = default(int);
		int num12 = default(int);
		double num13 = default(double);
		int num14 = default(int);
		double num15 = default(double);
		int num16 = default(int);
		double num17 = default(double);
		int num18 = default(int);
		double num19 = default(double);
		double num20 = default(double);
		double num21 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1029:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_0017;
							case 5:
								goto IL_0024;
							case 6:
								goto IL_0039;
							case 7:
								goto IL_004e;
							case 8:
								goto IL_0061;
							case 9:
								goto IL_0074;
							case 10:
								goto IL_0082;
							case 11:
								goto IL_0095;
							case 12:
								goto IL_00b6;
							case 13:
								goto IL_00d8;
							case 14:
								goto IL_00e7;
							case 15:
								goto IL_00fa;
							case 16:
								goto IL_010e;
							case 17:
								goto IL_0120;
							case 18:
								goto IL_0134;
							case 19:
								goto IL_014d;
							case 21:
								goto IL_0168;
							case 22:
								goto IL_016e;
							case 20:
							case 23:
								goto IL_0175;
							case 24:
								goto IL_017b;
							case 25:
								goto IL_0182;
							case 26:
								goto IL_01a0;
							case 27:
								goto IL_01b2;
							case 28:
								goto IL_01c6;
							case 29:
								goto IL_01d9;
							case 30:
								goto IL_01ed;
							case 31:
								goto IL_0206;
							case 32:
								goto IL_021f;
							case 33:
								goto IL_022a;
							case 34:
								goto IL_0244;
							case 35:
								goto IL_0256;
							case 36:
								goto IL_026a;
							case 37:
								goto IL_0283;
							case 38:
								goto IL_0297;
							case 39:
								goto IL_02b1;
							case 40:
								goto IL_02cb;
							case 41:
								goto IL_02df;
							case 42:
								goto IL_02f8;
							case 43:
								goto IL_030a;
							case 44:
								goto IL_0315;
							case 45:
								goto IL_031e;
							case 46:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 47:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_0182:
						num2 = 25;
						i++;
						goto IL_0193;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						num5 = 64;
						goto IL_0017;
						IL_0017:
						num2 = 4;
						num6 = 0.99;
						goto IL_0024;
						IL_0024:
						num2 = 5;
						num7 = EllipseStart.X - 1.0;
						goto IL_0039;
						IL_0039:
						num2 = 6;
						num8 = EllipseStart.Y - 1.0;
						goto IL_004e;
						IL_004e:
						num2 = 7;
						num9 = ELlipseEnd.X - EllipseStart.X;
						goto IL_0061;
						IL_0061:
						num2 = 8;
						num10 = ELlipseEnd.Y - EllipseStart.Y;
						goto IL_0074;
						IL_0074:
						num2 = 9;
						pts = new StylusPointCollection();
						goto IL_0082;
						IL_0082:
						num2 = 10;
						num11 = num5;
						i = 0;
						goto IL_0193;
						IL_0193:
						if (i <= num11)
						{
							goto IL_0095;
						}
						goto IL_01a0;
						IL_01a0:
						num2 = 26;
						num12 = (int)Math.Round(num7 + num13 + num6);
						goto IL_01b2;
						IL_01b2:
						num2 = 27;
						num14 = (int)Math.Round(num8 + (0.0 - num15) + num6);
						goto IL_01c6;
						IL_01c6:
						num2 = 28;
						num16 = (int)Math.Round(num7 + num17 + num6);
						goto IL_01d9;
						IL_01d9:
						num2 = 29;
						num18 = (int)Math.Round(num8 + (0.0 - num19) + num6);
						goto IL_01ed;
						IL_01ed:
						num2 = 30;
						pts.Add(new StylusPoint(num12, num14));
						goto IL_0206;
						IL_0206:
						num2 = 31;
						pts.Add(new StylusPoint(num16, num18));
						goto IL_021f;
						IL_021f:
						num2 = 32;
						if (st != null)
						{
							goto IL_022a;
						}
						goto IL_0256;
						IL_022a:
						num2 = 33;
						inkCanvas.Strokes.Remove(st);
						goto IL_0244;
						IL_0244:
						num2 = 34;
						FrameNo--;
						goto IL_0256;
						IL_0256:
						num2 = 35;
						st = new Stroke(pts);
						goto IL_026a;
						IL_026a:
						num2 = 36;
						st.DrawingAttributes.Color = InkColor;
						goto IL_0283;
						IL_0283:
						num2 = 37;
						st.DrawingAttributes.FitToCurve = true;
						goto IL_0297;
						IL_0297:
						num2 = 38;
						st.DrawingAttributes.Width = unchecked((int)ShapeSize);
						goto IL_02b1;
						IL_02b1:
						num2 = 39;
						st.DrawingAttributes.Height = unchecked((int)ShapeSize);
						goto IL_02cb;
						IL_02cb:
						num2 = 40;
						st.DrawingAttributes.StylusTip = StylusTip.Ellipse;
						goto IL_02df;
						IL_02df:
						num2 = 41;
						inkCanvas.Strokes.Add(st);
						goto IL_02f8;
						IL_02f8:
						num2 = 42;
						FrameNo++;
						goto IL_030a;
						IL_030a:
						num2 = 43;
						if (iRedo != 0)
						{
							goto IL_0315;
						}
						goto IL_031e;
						IL_0315:
						num2 = 44;
						ResetRedo();
						goto IL_031e;
						IL_031e:
						num2 = 45;
						if (FrameNo != 1)
						{
							goto end_IL_0000_3;
						}
						break;
						IL_0095:
						num2 = 11;
						num13 = num9 * Math.Sin((double)i / (double)num5 * (Math.PI * 2.0));
						goto IL_00b6;
						IL_00b6:
						num2 = 12;
						num15 = num10 * Math.Cos((double)i / (double)num5 * (Math.PI * 2.0));
						goto IL_00d8;
						IL_00d8:
						num2 = 13;
						if (i > 0)
						{
							goto IL_00e7;
						}
						goto IL_0168;
						IL_00e7:
						num2 = 14;
						num12 = (int)Math.Round(num7 + num20 + num6);
						goto IL_00fa;
						IL_00fa:
						num2 = 15;
						num14 = (int)Math.Round(num8 + (0.0 - num21) + num6);
						goto IL_010e;
						IL_010e:
						num2 = 16;
						num16 = (int)Math.Round(num7 + num13 + num6);
						goto IL_0120;
						IL_0120:
						num2 = 17;
						num18 = (int)Math.Round(num8 + (0.0 - num15) + num6);
						goto IL_0134;
						IL_0134:
						num2 = 18;
						pts.Add(new StylusPoint(num12, num14));
						goto IL_014d;
						IL_014d:
						num2 = 19;
						pts.Add(new StylusPoint(num16, num18));
						goto IL_0175;
						IL_0168:
						num2 = 21;
						num17 = num13;
						goto IL_016e;
						IL_016e:
						num2 = 22;
						num19 = num15;
						goto IL_0175;
						IL_0175:
						num2 = 23;
						num20 = num13;
						goto IL_017b;
						IL_017b:
						num2 = 24;
						num21 = num15;
						goto IL_0182;
						end_IL_0000_2:
						break;
					}
					num2 = 46;
					SelectUndoImageOfFav();
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1029;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void DrawLine(Point LineStart, Point LineEnd)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 449:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_001e;
							case 5:
								goto IL_003e;
							case 6:
								goto IL_005e;
							case 7:
								goto IL_0068;
							case 8:
								goto IL_0081;
							case 9:
								goto IL_0092;
							case 10:
								goto IL_00a6;
							case 11:
								goto IL_00bf;
							case 12:
								goto IL_00d9;
							case 13:
								goto IL_00f3;
							case 14:
								goto IL_0107;
							case 15:
								goto IL_0120;
							case 16:
								goto IL_0132;
							case 17:
								goto IL_013d;
							case 18:
								goto IL_0146;
							case 19:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 20:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_0146:
						num2 = 18;
						if (FrameNo != 1)
						{
							goto end_IL_0000_3;
						}
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						pts = new StylusPointCollection();
						goto IL_001e;
						IL_001e:
						num2 = 4;
						pts.Add(new StylusPoint(LineStart.X, LineStart.Y));
						goto IL_003e;
						IL_003e:
						num2 = 5;
						pts.Add(new StylusPoint(LineEnd.X, LineEnd.Y));
						goto IL_005e;
						IL_005e:
						num2 = 6;
						if (st != null)
						{
							goto IL_0068;
						}
						goto IL_0092;
						IL_0068:
						num2 = 7;
						inkCanvas.Strokes.Remove(st);
						goto IL_0081;
						IL_0081:
						num2 = 8;
						FrameNo--;
						goto IL_0092;
						IL_0092:
						num2 = 9;
						st = new Stroke(pts);
						goto IL_00a6;
						IL_00a6:
						num2 = 10;
						st.DrawingAttributes.Color = InkColor;
						goto IL_00bf;
						IL_00bf:
						num2 = 11;
						st.DrawingAttributes.Width = unchecked((int)ShapeSize);
						goto IL_00d9;
						IL_00d9:
						num2 = 12;
						st.DrawingAttributes.Height = unchecked((int)ShapeSize);
						goto IL_00f3;
						IL_00f3:
						num2 = 13;
						st.DrawingAttributes.StylusTip = StylusTip.Ellipse;
						goto IL_0107;
						IL_0107:
						num2 = 14;
						inkCanvas.Strokes.Add(st);
						goto IL_0120;
						IL_0120:
						num2 = 15;
						FrameNo++;
						goto IL_0132;
						IL_0132:
						num2 = 16;
						if (iRedo != 0)
						{
							goto IL_013d;
						}
						goto IL_0146;
						IL_013d:
						num2 = 17;
						ResetRedo();
						goto IL_0146;
						end_IL_0000_2:
						break;
					}
					num2 = 19;
					SelectUndoImageOfFav();
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 449;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void DrawRectangle(Point RectangleStart, Point RectangleEnd)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 560:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_001e;
							case 5:
								goto IL_003e;
							case 6:
								goto IL_005e;
							case 7:
								goto IL_007e;
							case 8:
								goto IL_009e;
							case 9:
								goto IL_00be;
							case 10:
								goto IL_00c9;
							case 11:
								goto IL_00e3;
							case 12:
								goto IL_00f5;
							case 13:
								goto IL_0109;
							case 14:
								goto IL_0122;
							case 15:
								goto IL_013c;
							case 16:
								goto IL_0156;
							case 17:
								goto IL_016a;
							case 18:
								goto IL_0183;
							case 19:
								goto IL_0195;
							case 20:
								goto IL_01a0;
							case 21:
								goto IL_01a9;
							case 22:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 23:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_01a9:
						num2 = 21;
						if (FrameNo != 1)
						{
							goto end_IL_0000_3;
						}
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						pts = new StylusPointCollection();
						goto IL_001e;
						IL_001e:
						num2 = 4;
						pts.Add(new StylusPoint(RectangleStart.X, RectangleStart.Y));
						goto IL_003e;
						IL_003e:
						num2 = 5;
						pts.Add(new StylusPoint(RectangleEnd.X, RectangleStart.Y));
						goto IL_005e;
						IL_005e:
						num2 = 6;
						pts.Add(new StylusPoint(RectangleEnd.X, RectangleEnd.Y));
						goto IL_007e;
						IL_007e:
						num2 = 7;
						pts.Add(new StylusPoint(RectangleStart.X, RectangleEnd.Y));
						goto IL_009e;
						IL_009e:
						num2 = 8;
						pts.Add(new StylusPoint(RectangleStart.X, RectangleStart.Y));
						goto IL_00be;
						IL_00be:
						num2 = 9;
						if (st != null)
						{
							goto IL_00c9;
						}
						goto IL_00f5;
						IL_00c9:
						num2 = 10;
						inkCanvas.Strokes.Remove(st);
						goto IL_00e3;
						IL_00e3:
						num2 = 11;
						FrameNo--;
						goto IL_00f5;
						IL_00f5:
						num2 = 12;
						st = new Stroke(pts);
						goto IL_0109;
						IL_0109:
						num2 = 13;
						st.DrawingAttributes.Color = InkColor;
						goto IL_0122;
						IL_0122:
						num2 = 14;
						st.DrawingAttributes.Width = unchecked((int)ShapeSize);
						goto IL_013c;
						IL_013c:
						num2 = 15;
						st.DrawingAttributes.Height = unchecked((int)ShapeSize);
						goto IL_0156;
						IL_0156:
						num2 = 16;
						st.DrawingAttributes.StylusTip = StylusTip.Rectangle;
						goto IL_016a;
						IL_016a:
						num2 = 17;
						inkCanvas.Strokes.Add(st);
						goto IL_0183;
						IL_0183:
						num2 = 18;
						FrameNo++;
						goto IL_0195;
						IL_0195:
						num2 = 19;
						if (iRedo != 0)
						{
							goto IL_01a0;
						}
						goto IL_01a9;
						IL_01a0:
						num2 = 20;
						ResetRedo();
						goto IL_01a9;
						end_IL_0000_2:
						break;
					}
					num2 = 22;
					SelectUndoImageOfFav();
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 560;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void DrawTriangle(Point Triangle1stPoint, Point Triangle2ndPoint, Point Triangle3rdPoint = default(Point))
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1003:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_001e;
							case 5:
								goto IL_002b;
							case 6:
								goto IL_004b;
							case 7:
								goto IL_006b;
							case 8:
								goto IL_0075;
							case 9:
								goto IL_008e;
							case 10:
								goto IL_00a2;
							case 11:
								goto IL_00bb;
							case 12:
								goto IL_00d5;
							case 13:
								goto IL_00ef;
							case 14:
								goto IL_0103;
							case 15:
								goto IL_011c;
							case 16:
								goto IL_012a;
							case 18:
								goto IL_0138;
							case 19:
								goto IL_0146;
							case 20:
								goto IL_0167;
							case 21:
								goto IL_0188;
							case 22:
								goto IL_01a9;
							case 23:
								goto IL_01ca;
							case 24:
								goto IL_01eb;
							case 25:
								goto IL_020c;
							case 26:
								goto IL_0217;
							case 27:
								goto IL_0231;
							case 28:
								goto IL_023c;
							case 29:
								goto IL_0256;
							case 30:
								goto IL_0268;
							case 31:
								goto IL_027c;
							case 32:
								goto IL_0295;
							case 33:
								goto IL_02af;
							case 34:
								goto IL_02c9;
							case 35:
								goto IL_02dd;
							case 36:
								goto IL_02f6;
							case 37:
								goto IL_0308;
							case 38:
								goto IL_0313;
							case 39:
								goto IL_031c;
							case 40:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 17:
							case 41:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_031c:
						num2 = 39;
						if (FrameNo != 1)
						{
							goto end_IL_0000_3;
						}
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						if (!isTrianglesSecondStep)
						{
							goto IL_001e;
						}
						goto IL_0138;
						IL_001e:
						num2 = 4;
						pts = new StylusPointCollection();
						goto IL_002b;
						IL_002b:
						num2 = 5;
						pts.Add(new StylusPoint(Triangle1stPoint.X, Triangle1stPoint.Y));
						goto IL_004b;
						IL_004b:
						num2 = 6;
						pts.Add(new StylusPoint(Triangle2ndPoint.X, Triangle2ndPoint.Y));
						goto IL_006b;
						IL_006b:
						num2 = 7;
						if (st != null)
						{
							goto IL_0075;
						}
						goto IL_008e;
						IL_0075:
						num2 = 8;
						inkCanvas.Strokes.Remove(st);
						goto IL_008e;
						IL_008e:
						num2 = 9;
						st = new Stroke(pts);
						goto IL_00a2;
						IL_00a2:
						num2 = 10;
						st.DrawingAttributes.Color = InkColor;
						goto IL_00bb;
						IL_00bb:
						num2 = 11;
						st.DrawingAttributes.Width = unchecked((int)ShapeSize);
						goto IL_00d5;
						IL_00d5:
						num2 = 12;
						st.DrawingAttributes.Height = unchecked((int)ShapeSize);
						goto IL_00ef;
						IL_00ef:
						num2 = 13;
						st.DrawingAttributes.StylusTip = StylusTip.Ellipse;
						goto IL_0103;
						IL_0103:
						num2 = 14;
						inkCanvas.Strokes.Add(st);
						goto IL_011c;
						IL_011c:
						num2 = 15;
						if (iRedo == 0)
						{
							goto end_IL_0000_3;
						}
						goto IL_012a;
						IL_012a:
						num2 = 16;
						ResetRedo();
						goto end_IL_0000_3;
						IL_0138:
						num2 = 18;
						pts = new StylusPointCollection();
						goto IL_0146;
						IL_0146:
						num2 = 19;
						pts.Add(new StylusPoint(Triangle1stPoint.X, Triangle1stPoint.Y));
						goto IL_0167;
						IL_0167:
						num2 = 20;
						pts.Add(new StylusPoint(Triangle2ndPoint.X, Triangle2ndPoint.Y));
						goto IL_0188;
						IL_0188:
						num2 = 21;
						pts.Add(new StylusPoint(Triangle1stPoint.X, Triangle1stPoint.Y));
						goto IL_01a9;
						IL_01a9:
						num2 = 22;
						pts.Add(new StylusPoint(Triangle3rdPoint.X, Triangle3rdPoint.Y));
						goto IL_01ca;
						IL_01ca:
						num2 = 23;
						pts.Add(new StylusPoint(Triangle2ndPoint.X, Triangle2ndPoint.Y));
						goto IL_01eb;
						IL_01eb:
						num2 = 24;
						pts.Add(new StylusPoint(Triangle3rdPoint.X, Triangle3rdPoint.Y));
						goto IL_020c;
						IL_020c:
						num2 = 25;
						if (st != null)
						{
							goto IL_0217;
						}
						goto IL_0231;
						IL_0217:
						num2 = 26;
						inkCanvas.Strokes.Remove(st);
						goto IL_0231;
						IL_0231:
						num2 = 27;
						if (st2 != null)
						{
							goto IL_023c;
						}
						goto IL_0268;
						IL_023c:
						num2 = 28;
						inkCanvas.Strokes.Remove(st2);
						goto IL_0256;
						IL_0256:
						num2 = 29;
						FrameNo--;
						goto IL_0268;
						IL_0268:
						num2 = 30;
						st2 = new Stroke(pts);
						goto IL_027c;
						IL_027c:
						num2 = 31;
						st2.DrawingAttributes.Color = InkColor;
						goto IL_0295;
						IL_0295:
						num2 = 32;
						st2.DrawingAttributes.Width = unchecked((int)ShapeSize);
						goto IL_02af;
						IL_02af:
						num2 = 33;
						st2.DrawingAttributes.Height = unchecked((int)ShapeSize);
						goto IL_02c9;
						IL_02c9:
						num2 = 34;
						st2.DrawingAttributes.StylusTip = StylusTip.Ellipse;
						goto IL_02dd;
						IL_02dd:
						num2 = 35;
						inkCanvas.Strokes.Add(st2);
						goto IL_02f6;
						IL_02f6:
						num2 = 36;
						FrameNo++;
						goto IL_0308;
						IL_0308:
						num2 = 37;
						if (iRedo != 0)
						{
							goto IL_0313;
						}
						goto IL_031c;
						IL_0313:
						num2 = 38;
						ResetRedo();
						goto IL_031c;
						end_IL_0000_2:
						break;
					}
					num2 = 40;
					SelectUndoImageOfFav();
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1003;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ResetSettingsWindow()
	{
		lblSettings.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 82, 82, 82));
		lblSettings.Background = new SolidColorBrush(Colors.White);
		lblSettingsStartingPen.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 82, 82, 82));
		lblSettingsStartingPosition.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 82, 82, 82));
		lblSettingsFavourites.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 82, 82, 82));
		lblSettingsSpeedAccess.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 82, 82, 82));
		lblSettingsCloseConfirmation.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 82, 82, 82));
		lblSettingsAutoStart.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 82, 82, 82));
		lblAbout.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 82, 82, 82));
		lblAbout.Background = new SolidColorBrush(Colors.White);
		GridLogo.Visibility = Visibility.Collapsed;
		GridStartingPen.Visibility = Visibility.Collapsed;
		GridStartingPosition.Visibility = Visibility.Collapsed;
		GridFavourites.Visibility = Visibility.Collapsed;
		GridSpeedAccess.Visibility = Visibility.Collapsed;
		GridCloseConfirmation.Visibility = Visibility.Collapsed;
		GridAutoStart.Visibility = Visibility.Collapsed;
		GridAbout.Visibility = Visibility.Collapsed;
	}

	private void imgCloseSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		borderSettings.Visibility = Visibility.Collapsed;
	}

	private void lblSettingsMenuSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (lblSettings.Background != Brushes.GhostWhite)
		{
			lblSettingsMenuStartingPen_MouseLeftButtonUp(null, null);
		}
	}

	private void lblSettingsMenuStartingPen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetSettingsWindow();
		lblSettingsStartingPen.Foreground = Brushes.DodgerBlue;
		lblSettings.Foreground = Brushes.DodgerBlue;
		lblSettings.Background = Brushes.GhostWhite;
		switch (MySettingsProperty.Settings.PenStyle)
		{
		case 0:
			lblPenType.Content = "Tahta Kalemi";
			break;
		case 1:
			lblPenType.Content = "Kesik Uçlu Kalem";
			break;
		case 2:
			lblPenType.Content = "Fosforlu Kalem";
			break;
		}
		lblInkSize.Content = "Boyut : " + MySettingsProperty.Settings.InkSize;
		SolidColorBrush fill = new SolidColorBrush
		{
			Color = MySettingsProperty.Settings.InkColor
		};
		ellipseInkColor.Fill = fill;
		AnimationFadeIn(GridStartingPen, 400);
	}

	private void lblSetDefaultPen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		AnimationFadeIn(RuntimeHelpers.GetObjectValue(sender), 800);
		lblPenType.Content = "Kesik Uçlu Kalem";
		lblInkSize.Content = "Boyut : " + Conversions.ToString(3);
		SolidColorBrush fill = new SolidColorBrush
		{
			Color = Color.FromArgb(byte.MaxValue, 0, 0, 157)
		};
		ellipseInkColor.Fill = fill;
		MySettingsProperty.Settings.PenStyle = 1;
		MySettingsProperty.Settings.InkSize = 3;
		MySettingsProperty.Settings.ColorNo = 2;
		MySettingsProperty.Settings.InkColor = Color.FromArgb(byte.MaxValue, 0, 0, 157);
		MySettingsProperty.Settings.Save();
	}

	private void lblSetCurrentPenState_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		AnimationFadeIn(RuntimeHelpers.GetObjectValue(sender), 800);
		switch (PenStyle)
		{
		case PenStyleEnum.Marker:
			lblPenType.Content = "Tahta Kalemi";
			break;
		case PenStyleEnum.Stylograph:
			lblPenType.Content = "Kesik Uçlu Kalem";
			break;
		case PenStyleEnum.Highlighter:
			lblPenType.Content = "Fosforlu Kalem";
			break;
		}
		lblInkSize.Content = "Boyut : " + InkSize;
		SolidColorBrush fill = new SolidColorBrush
		{
			Color = InkColor
		};
		ellipseInkColor.Fill = fill;
		MySettingsProperty.Settings.PenStyle = (byte)PenStyle;
		MySettingsProperty.Settings.InkSize = InkSize;
		MySettingsProperty.Settings.ColorNo = ColorNo;
		MySettingsProperty.Settings.InkColor = InkColor;
		MySettingsProperty.Settings.Save();
	}

	private void lblSettingsMenuStartingPosition_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetSettingsWindow();
		lblSettingsStartingPosition.Foreground = Brushes.DodgerBlue;
		lblSettings.Foreground = Brushes.DodgerBlue;
		lblSettings.Background = Brushes.GhostWhite;
		checked
		{
			if (MySettingsProperty.Settings.isStartupPositionDefault)
			{
				imgMiniMainMenu.Margin = new Thickness(rectScreen.Margin.Left + 2.0, (int)Math.Round((rectScreen.Height - imgMiniMainMenu.Height) * 1.0 / 3.0 + rectScreen.Margin.Top + 2.0), 0.0, 0.0);
			}
			else
			{
				int num = (int)Math.Round(rectScreen.Margin.Left + (double)MySettingsProperty.Settings.Left * (rectScreen.Width / base.Width) + 1.0);
				if ((double)num < rectScreen.Margin.Left)
				{
					num = (int)Math.Round(rectScreen.Margin.Left);
				}
				if ((double)num > rectScreen.Margin.Left + rectScreen.Width - imgMiniMainMenu.Width)
				{
					num = (int)Math.Round(rectScreen.Margin.Left + rectScreen.Width - imgMiniMainMenu.Width);
				}
				int num2 = (int)Math.Round(rectScreen.Margin.Top + (double)MySettingsProperty.Settings.Top * (rectScreen.Height / base.Height) + 1.0);
				if ((double)num2 < rectScreen.Margin.Top)
				{
					num2 = (int)Math.Round(rectScreen.Margin.Top);
				}
				if ((double)num2 > rectScreen.Margin.Top + rectScreen.Height - imgMiniMainMenu.Height)
				{
					num2 = (int)Math.Round(rectScreen.Margin.Top + rectScreen.Height - imgMiniMainMenu.Height);
				}
				imgMiniMainMenu.Margin = new Thickness(num, num2, 0.0, 0.0);
			}
			AnimationFadeIn(GridStartingPosition, 400);
		}
	}

	private void lblSetCurrentPosition_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		AnimationFadeIn(RuntimeHelpers.GetObjectValue(sender), 800);
		checked
		{
			int num = (int)Math.Round(rectScreen.Margin.Left + (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left) * (rectScreen.Width / base.Width) + 1.0);
			if ((double)num < rectScreen.Margin.Left)
			{
				num = (int)Math.Round(rectScreen.Margin.Left);
			}
			if ((double)num > rectScreen.Margin.Left + rectScreen.Width - imgMiniMainMenu.Width)
			{
				num = (int)Math.Round(rectScreen.Margin.Left + rectScreen.Width - imgMiniMainMenu.Width);
			}
			int num2 = (int)Math.Round(rectScreen.Margin.Top + (cnvMainMenu.Margin.Top + borderMainMenu.Margin.Top) * (rectScreen.Height / base.Height) + 1.0);
			if ((double)num2 < rectScreen.Margin.Top)
			{
				num2 = (int)Math.Round(rectScreen.Margin.Top);
			}
			if ((double)num2 > rectScreen.Margin.Top + rectScreen.Height - imgMiniMainMenu.Height)
			{
				num2 = (int)Math.Round(rectScreen.Margin.Top + rectScreen.Height - imgMiniMainMenu.Height);
			}
			imgMiniMainMenu.Margin = new Thickness(num, num2, 0.0, 0.0);
			MySettingsProperty.Settings.isStartupPositionDefault = false;
			MySettingsProperty.Settings.Left = (int)Math.Round(cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left);
			MySettingsProperty.Settings.Top = (int)Math.Round(cnvMainMenu.Margin.Top + borderMainMenu.Margin.Top);
			MySettingsProperty.Settings.Save();
		}
	}

	private void lblSetDefaultPosition_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		AnimationFadeIn(RuntimeHelpers.GetObjectValue(sender), 800);
		imgMiniMainMenu.Margin = new Thickness(rectScreen.Margin.Left + 2.0, checked((int)Math.Round((rectScreen.Height - imgMiniMainMenu.Height) * 1.0 / 3.0 + rectScreen.Margin.Top + 2.0)), 0.0, 0.0);
		MySettingsProperty.Settings.isStartupPositionDefault = true;
		MySettingsProperty.Settings.Save();
	}

	private void lblSettingsFavourites_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetSettingsWindow();
		lblSettingsFavourites.Foreground = Brushes.DodgerBlue;
		lblSettings.Foreground = Brushes.DodgerBlue;
		lblSettings.Background = Brushes.GhostWhite;
		AnimationFadeIn(GridFavourites, 400);
	}

	private void lblSaveFavourites_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		AnimationFadeIn(RuntimeHelpers.GetObjectValue(sender), 800);
		MySettingsProperty.Settings.iFav = iFav;
		if (imgFav[1] != null)
		{
			MySettingsProperty.Settings.Fav1 = Conversions.ToString(imgFav[1].Tag);
		}
		if (imgFav[2] != null)
		{
			MySettingsProperty.Settings.Fav2 = Conversions.ToString(imgFav[2].Tag);
		}
		if (imgFav[3] != null)
		{
			MySettingsProperty.Settings.Fav3 = Conversions.ToString(imgFav[3].Tag);
		}
		if (imgFav[4] != null)
		{
			MySettingsProperty.Settings.Fav4 = Conversions.ToString(imgFav[4].Tag);
		}
		if (imgFav[5] != null)
		{
			MySettingsProperty.Settings.Fav5 = Conversions.ToString(imgFav[5].Tag);
		}
		if (imgFav[6] != null)
		{
			MySettingsProperty.Settings.Fav6 = Conversions.ToString(imgFav[6].Tag);
		}
		if (imgFav[7] != null)
		{
			MySettingsProperty.Settings.Fav7 = Conversions.ToString(imgFav[7].Tag);
		}
		if (imgFav[8] != null)
		{
			MySettingsProperty.Settings.Fav8 = Conversions.ToString(imgFav[8].Tag);
		}
		if (imgFav[9] != null)
		{
			MySettingsProperty.Settings.Fav9 = Conversions.ToString(imgFav[9].Tag);
		}
		if (imgFav[10] != null)
		{
			MySettingsProperty.Settings.Fav10 = Conversions.ToString(imgFav[10].Tag);
		}
		if (imgFav[11] != null)
		{
			MySettingsProperty.Settings.Fav11 = Conversions.ToString(imgFav[11].Tag);
		}
		if (imgFav[12] != null)
		{
			MySettingsProperty.Settings.Fav12 = Conversions.ToString(imgFav[12].Tag);
		}
		if (imgFav[13] != null)
		{
			MySettingsProperty.Settings.Fav13 = Conversions.ToString(imgFav[13].Tag);
		}
		if (imgFav[14] != null)
		{
			MySettingsProperty.Settings.Fav14 = Conversions.ToString(imgFav[14].Tag);
		}
		if (imgFav[15] != null)
		{
			MySettingsProperty.Settings.Fav15 = Conversions.ToString(imgFav[15].Tag);
		}
		if (imgFav[16] != null)
		{
			MySettingsProperty.Settings.Fav16 = Conversions.ToString(imgFav[16].Tag);
		}
		if (imgFav[17] != null)
		{
			MySettingsProperty.Settings.Fav17 = Conversions.ToString(imgFav[17].Tag);
		}
		if (imgFav[18] != null)
		{
			MySettingsProperty.Settings.Fav18 = Conversions.ToString(imgFav[18].Tag);
		}
		if (imgFav[19] != null)
		{
			MySettingsProperty.Settings.Fav19 = Conversions.ToString(imgFav[19].Tag);
		}
		if (imgFav[20] != null)
		{
			MySettingsProperty.Settings.Fav20 = Conversions.ToString(imgFav[20].Tag);
		}
		MySettingsProperty.Settings.Save();
	}

	private void lblSetDefaultFavourites_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		AnimationFadeIn(RuntimeHelpers.GetObjectValue(sender), 800);
		MySettingsProperty.Settings.iFav = 0;
		MySettingsProperty.Settings.Fav1 = "";
		MySettingsProperty.Settings.Fav2 = "";
		MySettingsProperty.Settings.Fav3 = "";
		MySettingsProperty.Settings.Fav4 = "";
		MySettingsProperty.Settings.Fav5 = "";
		MySettingsProperty.Settings.Fav6 = "";
		MySettingsProperty.Settings.Fav7 = "";
		MySettingsProperty.Settings.Fav8 = "";
		MySettingsProperty.Settings.Fav9 = "";
		MySettingsProperty.Settings.Fav10 = "";
		MySettingsProperty.Settings.Fav11 = "";
		MySettingsProperty.Settings.Fav12 = "";
		MySettingsProperty.Settings.Fav13 = "";
		MySettingsProperty.Settings.Fav14 = "";
		MySettingsProperty.Settings.Fav15 = "";
		MySettingsProperty.Settings.Fav16 = "";
		MySettingsProperty.Settings.Fav17 = "";
		MySettingsProperty.Settings.Fav18 = "";
		MySettingsProperty.Settings.Fav19 = "";
		MySettingsProperty.Settings.Fav20 = "";
		MySettingsProperty.Settings.Save();
	}

	private void chkShortGesture_Click(object sender, RoutedEventArgs e)
	{
		if (chkShortGesture.IsChecked == true)
		{
			MySettingsProperty.Settings.isShortGestureEnabled = true;
			isShortGestureEnabled = true;
		}
		else
		{
			MySettingsProperty.Settings.isShortGestureEnabled = false;
			isShortGestureEnabled = false;
		}
		MySettingsProperty.Settings.Save();
	}

	private void chkSideArrows_Click(object sender, RoutedEventArgs e)
	{
		if (chkSideArrows.IsChecked == true)
		{
			MySettingsProperty.Settings.isSideArrowsEnabled = true;
			isSideArrowsEnabled = true;
			borderSideArrowRight.Margin = new Thickness(base.Width - imgSideArrowRight.Width, base.Height - imgSideArrowRight.Height, 0.0, 0.0);
			AnimationFadeIn(borderSideArrowRight, 500, borderSideArrowRight.Opacity);
			borderSideArrowLeft.Margin = new Thickness(0.0, base.Height - imgSideArrowLeft.Height, 0.0, 0.0);
			AnimationFadeIn(borderSideArrowLeft, 500, borderSideArrowLeft.Opacity);
		}
		else
		{
			MySettingsProperty.Settings.isSideArrowsEnabled = false;
			isSideArrowsEnabled = false;
			AnimationFadeOut(borderSideArrowRight, 500, borderSideArrowRight.Opacity);
			AnimationFadeOut(borderSideArrowLeft, 500, borderSideArrowLeft.Opacity);
		}
		MySettingsProperty.Settings.Save();
	}

	private void chkGestures_Click(object sender, RoutedEventArgs e)
	{
		if (chkGesture.IsChecked == true)
		{
			MySettingsProperty.Settings.isGestureEnabled = true;
			isGestureEnabled = true;
			chkShortGesture.IsEnabled = true;
		}
		else
		{
			MySettingsProperty.Settings.isGestureEnabled = false;
			isGestureEnabled = false;
			chkShortGesture.IsEnabled = false;
		}
		MySettingsProperty.Settings.Save();
	}

	private void lblSettingsMenuSpeedAccess_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetSettingsWindow();
		lblSettingsSpeedAccess.Foreground = Brushes.DodgerBlue;
		lblSettings.Foreground = Brushes.DodgerBlue;
		lblSettings.Background = Brushes.GhostWhite;
		chkGesture.IsChecked = isGestureEnabled;
		chkShortGesture.IsChecked = isShortGestureEnabled;
		chkSideArrows.IsChecked = isSideArrowsEnabled;
		if (!chkGesture.IsChecked == true)
		{
			chkShortGesture.IsEnabled = false;
		}
		AnimationFadeIn(GridSpeedAccess, 400);
	}

	private void chkCloseConfirmation_Click(object sender, RoutedEventArgs e)
	{
		MySettingsProperty.Settings.isCLoseConfirmationEnabled = chkCloseConfirmation.IsChecked.Value;
		MySettingsProperty.Settings.Save();
	}

	private void lblSettingsCloseConfirmation_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetSettingsWindow();
		lblSettingsCloseConfirmation.Foreground = Brushes.DodgerBlue;
		lblSettings.Foreground = Brushes.DodgerBlue;
		lblSettings.Background = Brushes.GhostWhite;
		chkCloseConfirmation.IsChecked = MySettingsProperty.Settings.isCLoseConfirmationEnabled;
		AnimationFadeIn(GridCloseConfirmation, 400);
	}

	private const string RunKeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

	private const string RunValueName = "Fatih Pen";

	private static string ExePath => AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName;

	private static string AutoStartCommand => "\"" + ExePath + "\"";

	private static string PreLoadCommand => "\"" + ExePath + "\" /PreLoad";

	private static string ReadRunValue()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: false);
			return registryKey?.GetValue(RunValueName) as string;
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static void WriteRunValue(string value)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(RunKeyPath);
			if (value == null)
			{
				registryKey?.DeleteValue(RunValueName, throwOnMissingValue: false);
			}
			else
			{
				registryKey?.SetValue(RunValueName, value);
			}
		}
		catch (Exception)
		{
		}
	}

	private void lblSettingsMenuAutoStart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetSettingsWindow();
		lblSettingsAutoStart.Foreground = Brushes.DodgerBlue;
		lblSettings.Foreground = Brushes.DodgerBlue;
		lblSettings.Background = Brushes.GhostWhite;
		string left = ReadRunValue();
		// Eski sürümlerin yazdığı tırnaksız değerler de tanınır.
		if (left == AutoStartCommand || left == ExePath)
		{
			chkAutoStart.IsChecked = true;
			chkPreLoad.IsEnabled = false;
			textChkPreLoad.Foreground = Brushes.LightGray;
		}
		else if (left == PreLoadCommand || left == ExePath + " /PreLoad")
		{
			chkPreLoad.IsChecked = true;
		}
		AnimationFadeIn(GridAutoStart, 400);
	}

	private void chkAutoStart_Click(object sender, RoutedEventArgs e)
	{
		if (chkAutoStart.IsChecked == true)
		{
			chkPreLoad.IsChecked = false;
			chkPreLoad.IsEnabled = false;
			textChkPreLoad.Foreground = Brushes.LightGray;
			WriteRunValue(AutoStartCommand);
		}
		else
		{
			chkPreLoad.IsEnabled = true;
			textChkPreLoad.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 82, 82, 82));
			WriteRunValue(null);
		}
	}

	private void chkPreLoad_Click(object sender, RoutedEventArgs e)
	{
		if (chkAutoStart.IsChecked == true)
		{
			return;
		}
		WriteRunValue((chkPreLoad.IsChecked == true) ? PreLoadCommand : null);
	}

	private void lblAboutMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetSettingsWindow();
		lblAbout.Foreground = Brushes.DodgerBlue;
		lblAbout.Background = Brushes.GhostWhite;
		AnimationFadeIn(GridAbout, 400);
	}

	private void lblWebLink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		AnimationFadeIn(RuntimeHelpers.GetObjectValue(sender), 800);
		Process.Start(GitHubUrl);
		imgHandAndPen_PreviewMouseLeftButtonUp(null, null);
	}

	private void GestureDown(Point CursorPosition)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		int num5 = default(int);
		int num6 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 1027:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 5:
							goto IL_001e;
						case 6:
							goto IL_0028;
						case 7:
							goto IL_0030;
						case 8:
							goto IL_003e;
						case 10:
							goto IL_0060;
						case 11:
							goto IL_0084;
						case 12:
							goto IL_00a7;
						case 13:
							goto IL_00cb;
						case 14:
							goto IL_00e4;
						case 15:
							goto IL_00fd;
						case 17:
							goto IL_011b;
						case 18:
							goto IL_0141;
						case 19:
							goto IL_0164;
						case 20:
							goto IL_0188;
						case 21:
							goto IL_01a1;
						case 22:
							goto IL_01ba;
						case 24:
							goto IL_01d8;
						case 25:
							goto IL_0200;
						case 26:
							goto IL_0228;
						case 27:
							goto IL_0250;
						case 28:
							goto IL_0269;
						case 29:
							goto IL_0282;
						case 9:
						case 16:
						case 23:
						case 30:
						case 31:
							goto IL_029b;
						case 32:
							goto IL_02a5;
						case 33:
							goto IL_02d0;
						case 34:
							goto IL_02fc;
						case 35:
							goto IL_0326;
						case 36:
							goto IL_033d;
						case 37:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 4:
						case 38:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_033d:
					num2 = 36;
					cnvGesture.Visibility = Visibility.Visible;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (!isGestureEnabled)
					{
						goto end_IL_0000_3;
					}
					goto IL_001e;
					IL_001e:
					num2 = 5;
					if (ShownSubMenu != null)
					{
						goto IL_0028;
					}
					goto IL_0030;
					IL_0028:
					num2 = 6;
					CollapseSubMenus();
					goto IL_0030;
					IL_0030:
					num2 = 7;
					inkCanvas.EditingMode = InkCanvasEditingMode.None;
					goto IL_003e;
					IL_003e:
					num2 = 8;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_011b;
					case PenStyleEnum.Highlighter:
						goto IL_01d8;
					default:
						goto IL_029b;
					}
					goto IL_0060;
					IL_01d8:
					num2 = 24;
					pathExpandingCircleLeft.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue));
					goto IL_0200;
					IL_0200:
					num2 = 25;
					pathExpandingCircleUp.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1));
					goto IL_0228;
					IL_0228:
					num2 = 26;
					pathExpandingCircleRight.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 1, byte.MaxValue, byte.MaxValue));
					goto IL_0250;
					IL_0250:
					num2 = 27;
					imgGestureLeft.Source = imgGestureHighlighterRed.Source;
					goto IL_0269;
					IL_0269:
					num2 = 28;
					imgGestureUp.Source = imgGestureHighlighterYellow.Source;
					goto IL_0282;
					IL_0282:
					num2 = 29;
					imgGestureRight.Source = imgGestureHighlighterBlue.Source;
					goto IL_029b;
					IL_011b:
					num2 = 17;
					pathExpandingCircleLeft.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 230, 27, 27));
					goto IL_0141;
					IL_0141:
					num2 = 18;
					pathExpandingCircleUp.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 20, 20, 20));
					goto IL_0164;
					IL_0164:
					num2 = 19;
					pathExpandingCircleRight.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 0, 0, 157));
					goto IL_0188;
					IL_0188:
					num2 = 20;
					imgGestureLeft.Source = imgGestureStylographRed.Source;
					goto IL_01a1;
					IL_01a1:
					num2 = 21;
					imgGestureUp.Source = imgGestureStylographBlack.Source;
					goto IL_01ba;
					IL_01ba:
					num2 = 22;
					imgGestureRight.Source = imgGestureStylographBlue.Source;
					goto IL_029b;
					IL_0060:
					num2 = 10;
					pathExpandingCircleLeft.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, 0));
					goto IL_0084;
					IL_0084:
					num2 = 11;
					pathExpandingCircleUp.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 20, 20, 20));
					goto IL_00a7;
					IL_00a7:
					num2 = 12;
					pathExpandingCircleRight.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 0, 0, byte.MaxValue));
					goto IL_00cb;
					IL_00cb:
					num2 = 13;
					imgGestureLeft.Source = imgGestureMarkerRed.Source;
					goto IL_00e4;
					IL_00e4:
					num2 = 14;
					imgGestureUp.Source = imgGestureMarkerBlack.Source;
					goto IL_00fd;
					IL_00fd:
					num2 = 15;
					imgGestureRight.Source = imgGestureMarkerBlue.Source;
					goto IL_029b;
					IL_029b:
					num2 = 31;
					pTouch2Start = CursorPosition;
					goto IL_02a5;
					IL_02a5:
					num2 = 32;
					num5 = checked((int)Math.Round(pTouch2Start.X - cnvGesture.Width / 2.0));
					goto IL_02d0;
					IL_02d0:
					num2 = 33;
					num6 = checked((int)Math.Round(pTouch2Start.Y - cnvGesture.Height / 2.0));
					goto IL_02fc;
					IL_02fc:
					num2 = 34;
					cnvGesture.Margin = new Thickness(num5, num6, 0.0, 0.0);
					goto IL_0326;
					IL_0326:
					num2 = 35;
					cnvGesture.Opacity = 0.0;
					goto IL_033d;
					end_IL_0000_2:
					break;
				}
				num2 = 37;
				isGestureVisible = true;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1027;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void GestureMove(Point CursorPosition)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point point = default(Point);
		double num5 = default(double);
		double num6 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 1100:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 5:
							goto IL_001e;
						case 6:
							goto IL_0027;
						case 7:
							goto IL_002b;
						case 8:
							goto IL_0042;
						case 9:
							goto IL_0059;
						case 10:
							goto IL_006f;
						case 11:
							goto IL_0082;
						case 12:
							goto IL_0092;
						case 13:
							goto IL_009b;
						case 14:
							goto IL_00aa;
						case 15:
							goto IL_00ba;
						case 16:
							goto IL_00c8;
						case 17:
							goto IL_00d2;
						case 18:
							goto IL_00e2;
						case 19:
							goto IL_00f2;
						case 21:
							goto IL_0129;
						case 22:
							goto IL_0139;
						case 23:
							goto IL_0142;
						case 24:
							goto IL_0151;
						case 25:
							goto IL_0161;
						case 26:
							goto IL_016f;
						case 27:
							goto IL_0179;
						case 28:
							goto IL_018e;
						case 29:
							goto IL_01a3;
						case 31:
							goto IL_01df;
						case 32:
							goto IL_01f2;
						case 33:
							goto IL_0202;
						case 34:
							goto IL_020b;
						case 35:
							goto IL_021a;
						case 36:
							goto IL_022a;
						case 37:
							goto IL_0238;
						case 38:
							goto IL_0242;
						case 39:
							goto IL_0252;
						case 40:
							goto IL_0262;
						case 42:
							goto IL_0299;
						case 43:
							goto IL_02a9;
						case 44:
							goto IL_02b2;
						case 45:
							goto IL_02c1;
						case 46:
							goto IL_02d1;
						case 47:
							goto IL_02df;
						case 48:
							goto IL_02e9;
						case 49:
							goto IL_02fe;
						case 50:
							goto IL_0313;
						case 20:
						case 30:
						case 41:
						case 51:
							goto IL_034a;
						case 52:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 4:
						case 53:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_034a:
					num2 = 51;
					cnvGesture.Visibility = Visibility.Visible;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (!isGestureEnabled)
					{
						goto end_IL_0000_3;
					}
					goto IL_001e;
					IL_001e:
					num2 = 5;
					SelectedGesture = GestureEnum.Abcent;
					goto IL_0027;
					IL_0027:
					num2 = 6;
					point = CursorPosition;
					goto IL_002b;
					IL_002b:
					num2 = 7;
					num5 = point.X - pTouch2Start.X;
					goto IL_0042;
					IL_0042:
					num2 = 8;
					num6 = point.Y - pTouch2Start.Y;
					goto IL_0059;
					IL_0059:
					num2 = 9;
					if (Math.Abs(num5) > Math.Abs(num6))
					{
						goto IL_006f;
					}
					goto IL_01df;
					IL_006f:
					num2 = 10;
					if (num5 > 0.0)
					{
						goto IL_0082;
					}
					goto IL_0129;
					IL_0082:
					num2 = 11;
					if (pathExpandingCircleRight.Visibility != Visibility.Visible)
					{
						goto IL_0092;
					}
					goto IL_00aa;
					IL_0092:
					num2 = 12;
					ResetExpandingCircle();
					goto IL_009b;
					IL_009b:
					num2 = 13;
					pathExpandingCircleRight.Visibility = Visibility.Visible;
					goto IL_00aa;
					IL_00aa:
					num2 = 14;
					if (num5 > 125.0)
					{
						goto IL_00ba;
					}
					goto IL_00d2;
					IL_00ba:
					num2 = 15;
					num5 = 125.0;
					goto IL_00c8;
					IL_00c8:
					num2 = 16;
					SelectedGesture = GestureEnum.Right;
					goto IL_00d2;
					IL_00d2:
					num2 = 17;
					ellipseExpandingRight.RadiusX = num5;
					goto IL_00e2;
					IL_00e2:
					num2 = 18;
					ellipseExpandingRight.RadiusY = num5;
					goto IL_00f2;
					IL_00f2:
					num2 = 19;
					cnvGesture.Opacity = 0.85 * Math.Pow(num5, 2.0) / 15625.0;
					goto IL_034a;
					IL_0129:
					num2 = 21;
					if (pathExpandingCircleLeft.Visibility != Visibility.Visible)
					{
						goto IL_0139;
					}
					goto IL_0151;
					IL_0139:
					num2 = 22;
					ResetExpandingCircle();
					goto IL_0142;
					IL_0142:
					num2 = 23;
					pathExpandingCircleLeft.Visibility = Visibility.Visible;
					goto IL_0151;
					IL_0151:
					num2 = 24;
					if (num5 < -125.0)
					{
						goto IL_0161;
					}
					goto IL_0179;
					IL_0161:
					num2 = 25;
					num5 = -125.0;
					goto IL_016f;
					IL_016f:
					num2 = 26;
					SelectedGesture = GestureEnum.Left;
					goto IL_0179;
					IL_0179:
					num2 = 27;
					ellipseExpandingLeft.RadiusX = Math.Abs(num5);
					goto IL_018e;
					IL_018e:
					num2 = 28;
					ellipseExpandingLeft.RadiusY = Math.Abs(num5);
					goto IL_01a3;
					IL_01a3:
					num2 = 29;
					cnvGesture.Opacity = 0.85 * Math.Pow(Math.Abs(num5), 2.0) / 15625.0;
					goto IL_034a;
					IL_01df:
					num2 = 31;
					if (num6 > 0.0)
					{
						goto IL_01f2;
					}
					goto IL_0299;
					IL_01f2:
					num2 = 32;
					if (pathExpandingCircleDown.Visibility != Visibility.Visible)
					{
						goto IL_0202;
					}
					goto IL_021a;
					IL_0202:
					num2 = 33;
					ResetExpandingCircle();
					goto IL_020b;
					IL_020b:
					num2 = 34;
					pathExpandingCircleDown.Visibility = Visibility.Visible;
					goto IL_021a;
					IL_021a:
					num2 = 35;
					if (num6 > 125.0)
					{
						goto IL_022a;
					}
					goto IL_0242;
					IL_022a:
					num2 = 36;
					num6 = 125.0;
					goto IL_0238;
					IL_0238:
					num2 = 37;
					SelectedGesture = GestureEnum.Down;
					goto IL_0242;
					IL_0242:
					num2 = 38;
					ellipseExpandingDown.RadiusX = num6;
					goto IL_0252;
					IL_0252:
					num2 = 39;
					ellipseExpandingDown.RadiusY = num6;
					goto IL_0262;
					IL_0262:
					num2 = 40;
					cnvGesture.Opacity = 0.85 * Math.Pow(num6, 2.0) / 15625.0;
					goto IL_034a;
					IL_0299:
					num2 = 42;
					if (pathExpandingCircleUp.Visibility != Visibility.Visible)
					{
						goto IL_02a9;
					}
					goto IL_02c1;
					IL_02a9:
					num2 = 43;
					ResetExpandingCircle();
					goto IL_02b2;
					IL_02b2:
					num2 = 44;
					pathExpandingCircleUp.Visibility = Visibility.Visible;
					goto IL_02c1;
					IL_02c1:
					num2 = 45;
					if (num6 < -125.0)
					{
						goto IL_02d1;
					}
					goto IL_02e9;
					IL_02d1:
					num2 = 46;
					num6 = -125.0;
					goto IL_02df;
					IL_02df:
					num2 = 47;
					SelectedGesture = GestureEnum.Up;
					goto IL_02e9;
					IL_02e9:
					num2 = 48;
					ellipseExpandingUp.RadiusX = Math.Abs(num6);
					goto IL_02fe;
					IL_02fe:
					num2 = 49;
					ellipseExpandingUp.RadiusY = Math.Abs(num6);
					goto IL_0313;
					IL_0313:
					num2 = 50;
					cnvGesture.Opacity = 0.85 * Math.Pow(Math.Abs(num6), 2.0) / 15625.0;
					goto IL_034a;
					end_IL_0000_2:
					break;
				}
				num2 = 52;
				isGestureVisible = true;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1100;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void GestureUp()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point point = default(Point);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				DrawStateEnum drawState;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 1890:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_003f;
						case 5:
							goto IL_0069;
						case 6:
							goto IL_0093;
						case 7:
							goto IL_00a1;
						case 8:
							goto IL_00ae;
						case 9:
							goto IL_00ca;
						case 10:
							goto IL_00dc;
						case 11:
							goto IL_012a;
						case 12:
							goto IL_0178;
						case 13:
							goto IL_0186;
						case 15:
							goto IL_019a;
						case 17:
							goto IL_01ab;
						case 14:
						case 16:
						case 18:
						case 19:
							goto IL_01ba;
						case 20:
							goto IL_01c9;
						case 21:
							goto IL_01d3;
						case 22:
							goto IL_01de;
						case 23:
							goto IL_01ec;
						case 25:
							goto IL_0215;
						case 26:
							goto IL_021f;
						case 27:
							goto IL_0228;
						case 28:
							goto IL_0234;
						case 29:
							goto IL_0274;
						case 31:
							goto IL_0285;
						case 30:
						case 32:
							goto IL_028e;
						case 33:
							goto IL_02a7;
						case 35:
							goto IL_0323;
						case 36:
							goto IL_032d;
						case 37:
							goto IL_0336;
						case 38:
							goto IL_0342;
						case 39:
							goto IL_0382;
						case 41:
							goto IL_0393;
						case 40:
						case 42:
							goto IL_039c;
						case 43:
							goto IL_03b5;
						case 45:
							goto IL_0431;
						case 46:
							goto IL_043d;
						case 48:
							goto IL_0449;
						case 47:
						case 49:
							goto IL_0453;
						case 50:
							goto IL_045c;
						case 51:
							goto IL_0468;
						case 52:
							goto IL_04a8;
						case 54:
							goto IL_04b9;
						case 53:
						case 55:
							goto IL_04c2;
						case 56:
							goto IL_04db;
						case 58:
							goto IL_0557;
						case 59:
							goto IL_0563;
						case 60:
							goto IL_0572;
						case 61:
							goto IL_0580;
						case 62:
							goto IL_0599;
						case 24:
						case 34:
						case 44:
						case 57:
						case 63:
						case 64:
							goto IL_0610;
						case 65:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 66:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_02a7:
					num2 = 33;
					imgGestureAnimation.Margin = new Thickness(cnvGesture.Margin.Left + imgGestureLeft.Margin.Left, cnvGesture.Margin.Top + imgGestureLeft.Margin.Top, 0.0, 0.0);
					goto IL_0610;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (isShortGestureEnabled & (cnvGesture.Opacity > 0.05) & (SelectedGesture == GestureEnum.Abcent))
					{
						goto IL_003f;
					}
					goto IL_0178;
					IL_003f:
					num2 = 4;
					point.X = pTouch2Start.X + borderMainMenu.Width / 2.0;
					goto IL_0069;
					IL_0069:
					num2 = 5;
					point.Y = pTouch2Start.Y - borderMainMenu.Height / 2.0;
					goto IL_0093;
					IL_0093:
					num2 = 6;
					tmrShortGesture.IsEnabled = true;
					goto IL_00a1;
					IL_00a1:
					num2 = 7;
					swShortGesture.Start();
					goto IL_00ae;
					IL_00ae:
					num2 = 8;
					tStart = swShortGesture.Elapsed.TotalMilliseconds;
					goto IL_00ca;
					IL_00ca:
					num2 = 9;
					tFirst = 0.0;
					goto IL_00dc;
					IL_00dc:
					num2 = 10;
					vX = 4.9 * (point.X - (cnvMainMenu.Margin.Left + borderMainMenu.Margin.Left)) / 1920.0;
					goto IL_012a;
					IL_012a:
					num2 = 11;
					vY = 4.9 * (point.Y - (cnvMainMenu.Margin.Top + borderMainMenu.Margin.Top)) / 1920.0;
					goto IL_0178;
					IL_0178:
					num2 = 12;
					if (!isGestureEnabled)
					{
						goto end_IL_0000_3;
					}
					goto IL_0186;
					IL_0186:
					num2 = 13;
					drawState = DrawState;
					if (drawState == DrawStateEnum.Pen)
					{
						goto IL_01ab;
					}
					if (drawState == DrawStateEnum.Eraser)
					{
						goto IL_019a;
					}
					goto IL_01ba;
					IL_0610:
					num2 = 64;
					imgGestureAnimation.Visibility = Visibility.Visible;
					break;
					IL_019a:
					num2 = 15;
					inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
					goto IL_01ba;
					IL_01ab:
					num2 = 17;
					inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
					goto IL_01ba;
					IL_01ba:
					num2 = 19;
					cnvGesture.Visibility = Visibility.Collapsed;
					goto IL_01c9;
					IL_01c9:
					num2 = 20;
					isGestureVisible = false;
					goto IL_01d3;
					IL_01d3:
					num2 = 21;
					GesturesStylusDeviceID = -2;
					goto IL_01de;
					IL_01de:
					num2 = 22;
					if (SelectedGesture == GestureEnum.Abcent)
					{
						goto end_IL_0000_3;
					}
					goto IL_01ec;
					IL_01ec:
					num2 = 23;
					switch (SelectedGesture)
					{
					case GestureEnum.Left:
						break;
					case GestureEnum.Right:
						goto IL_0323;
					case GestureEnum.Up:
						goto IL_0431;
					case GestureEnum.Down:
						goto IL_0557;
					default:
						goto IL_0610;
					}
					goto IL_0215;
					IL_0557:
					num2 = 58;
					if (DrawState != DrawStateEnum.Eraser)
					{
						goto IL_0563;
					}
					goto IL_0572;
					IL_0563:
					num2 = 59;
					DrawStateBeforeAction = DrawState;
					goto IL_0572;
					IL_0572:
					num2 = 60;
					ActivateEraser(62, 103, isGestureEraser: true);
					goto IL_0580;
					IL_0580:
					num2 = 61;
					imgGestureAnimation.Source = imgGestureDown.Source;
					goto IL_0599;
					IL_0599:
					num2 = 62;
					imgGestureAnimation.Margin = new Thickness(cnvGesture.Margin.Left + imgGestureDown.Margin.Left, cnvGesture.Margin.Top + imgGestureDown.Margin.Top, 0.0, 0.0);
					goto IL_0610;
					IL_0431:
					num2 = 45;
					if (PenStyle == PenStyleEnum.Highlighter)
					{
						goto IL_043d;
					}
					goto IL_0449;
					IL_043d:
					num2 = 46;
					ChangeColor(4);
					goto IL_0453;
					IL_0449:
					num2 = 48;
					ChangeColor(5);
					goto IL_0453;
					IL_0453:
					num2 = 49;
					ChangePenImage();
					goto IL_045c;
					IL_045c:
					num2 = 50;
					if (DrawState == DrawStateEnum.Eraser)
					{
						goto IL_0468;
					}
					goto IL_04c2;
					IL_0468:
					num2 = 51;
					if ((DrawStateBeforeAction == DrawStateEnum.Arrow) | (DrawStateBeforeAction == DrawStateEnum.DashLine) | (DrawStateBeforeAction == DrawStateEnum.Ellipse) | (DrawStateBeforeAction == DrawStateEnum.Line) | (DrawStateBeforeAction == DrawStateEnum.Rectangle) | (DrawStateBeforeAction == DrawStateEnum.Triangle))
					{
						goto IL_04a8;
					}
					goto IL_04b9;
					IL_04a8:
					num2 = 52;
					ActivateShape(DrawStateBeforeAction);
					goto IL_04c2;
					IL_04b9:
					num2 = 54;
					ActivatePen();
					goto IL_04c2;
					IL_04c2:
					num2 = 55;
					imgGestureAnimation.Source = imgGestureUp.Source;
					goto IL_04db;
					IL_04db:
					num2 = 56;
					imgGestureAnimation.Margin = new Thickness(cnvGesture.Margin.Left + imgGestureUp.Margin.Left, cnvGesture.Margin.Top + imgGestureUp.Margin.Top, 0.0, 0.0);
					goto IL_0610;
					IL_0323:
					num2 = 35;
					ChangeColor(2);
					goto IL_032d;
					IL_032d:
					num2 = 36;
					ChangePenImage();
					goto IL_0336;
					IL_0336:
					num2 = 37;
					if (DrawState == DrawStateEnum.Eraser)
					{
						goto IL_0342;
					}
					goto IL_039c;
					IL_0342:
					num2 = 38;
					if ((DrawStateBeforeAction == DrawStateEnum.Arrow) | (DrawStateBeforeAction == DrawStateEnum.DashLine) | (DrawStateBeforeAction == DrawStateEnum.Ellipse) | (DrawStateBeforeAction == DrawStateEnum.Line) | (DrawStateBeforeAction == DrawStateEnum.Rectangle) | (DrawStateBeforeAction == DrawStateEnum.Triangle))
					{
						goto IL_0382;
					}
					goto IL_0393;
					IL_0382:
					num2 = 39;
					ActivateShape(DrawStateBeforeAction);
					goto IL_039c;
					IL_0393:
					num2 = 41;
					ActivatePen();
					goto IL_039c;
					IL_039c:
					num2 = 42;
					imgGestureAnimation.Source = imgGestureRight.Source;
					goto IL_03b5;
					IL_03b5:
					num2 = 43;
					imgGestureAnimation.Margin = new Thickness(cnvGesture.Margin.Left + imgGestureRight.Margin.Left, cnvGesture.Margin.Top + imgGestureRight.Margin.Top, 0.0, 0.0);
					goto IL_0610;
					IL_0215:
					num2 = 25;
					ChangeColor(1);
					goto IL_021f;
					IL_021f:
					num2 = 26;
					ChangePenImage();
					goto IL_0228;
					IL_0228:
					num2 = 27;
					if (DrawState == DrawStateEnum.Eraser)
					{
						goto IL_0234;
					}
					goto IL_028e;
					IL_0234:
					num2 = 28;
					if ((DrawStateBeforeAction == DrawStateEnum.Arrow) | (DrawStateBeforeAction == DrawStateEnum.DashLine) | (DrawStateBeforeAction == DrawStateEnum.Ellipse) | (DrawStateBeforeAction == DrawStateEnum.Line) | (DrawStateBeforeAction == DrawStateEnum.Rectangle) | (DrawStateBeforeAction == DrawStateEnum.Triangle))
					{
						goto IL_0274;
					}
					goto IL_0285;
					IL_0274:
					num2 = 29;
					ActivateShape(DrawStateBeforeAction);
					goto IL_028e;
					IL_0285:
					num2 = 31;
					ActivatePen();
					goto IL_028e;
					IL_028e:
					num2 = 32;
					imgGestureAnimation.Source = imgGestureLeft.Source;
					goto IL_02a7;
					end_IL_0000_2:
					break;
				}
				num2 = 65;
				AnimationFadeOut(imgGestureAnimation, 1250);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1890;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ResetExpandingCircle()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 119:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001f;
						case 5:
							goto IL_002d;
						case 6:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 7:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_002d:
					num2 = 5;
					pathExpandingCircleLeft.Visibility = Visibility.Collapsed;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					pathExpandingCircleDown.Visibility = Visibility.Collapsed;
					goto IL_001f;
					IL_001f:
					num2 = 4;
					pathExpandingCircleUp.Visibility = Visibility.Collapsed;
					goto IL_002d;
					end_IL_0000_2:
					break;
				}
				num2 = 6;
				pathExpandingCircleRight.Visibility = Visibility.Collapsed;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 119;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void tmrShortGesture_Tick(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double num5 = default(double);
		double y = default(double);
		double num6 = default(double);
		double num7 = default(double);
		int num8 = default(int);
		int num9 = default(int);
		int num10 = default(int);
		bool flag = default(bool);
		int num11 = default(int);
		int num12 = default(int);
		int num13 = default(int);
		bool flag2 = default(bool);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1882:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_0030;
							case 5:
								goto IL_003d;
							case 6:
								goto IL_004a;
							case 7:
								goto IL_0057;
							case 8:
								goto IL_0076;
							case 9:
								goto IL_0095;
							case 10:
								goto IL_00ac;
							case 11:
								goto IL_00de;
							case 12:
								goto IL_0118;
							case 13:
								goto IL_0122;
							case 14:
								goto IL_012a;
							case 15:
								goto IL_0130;
							case 16:
								goto IL_0135;
							case 17:
								goto IL_017c;
							case 18:
								goto IL_01dc;
							case 19:
								goto IL_01e2;
							case 21:
								goto IL_01f7;
							case 22:
								goto IL_01ff;
							case 23:
								goto IL_0231;
							case 24:
								goto IL_026b;
							case 25:
								goto IL_0275;
							case 26:
								goto IL_027d;
							case 27:
								goto IL_0283;
							case 28:
								goto IL_0288;
							case 29:
								goto IL_02c3;
							case 30:
								goto IL_0311;
							case 31:
								goto IL_0317;
							case 20:
							case 32:
								goto IL_0327;
							case 33:
								goto IL_033e;
							case 34:
								goto IL_0370;
							case 35:
								goto IL_03aa;
							case 36:
								goto IL_03b4;
							case 37:
								goto IL_03bc;
							case 38:
								goto IL_03c2;
							case 39:
								goto IL_03c8;
							case 40:
								goto IL_040f;
							case 41:
								goto IL_046f;
							case 42:
								goto IL_0475;
							case 44:
								goto IL_048a;
							case 45:
								goto IL_0492;
							case 46:
								goto IL_04c4;
							case 47:
								goto IL_04fe;
							case 48:
								goto IL_0508;
							case 49:
								goto IL_0510;
							case 50:
								goto IL_0516;
							case 51:
								goto IL_051c;
							case 52:
								goto IL_0557;
							case 53:
								goto IL_05b9;
							case 54:
								goto IL_05bf;
							case 43:
							case 55:
								goto IL_05cf;
							case 56:
								goto IL_0624;
							case 57:
								goto IL_062d;
							case 58:
								goto IL_063c;
							case 59:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 60:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_063c:
						num2 = 58;
						swShortGesture.Stop();
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						num5 = swShortGesture.Elapsed.TotalMilliseconds - tStart;
						goto IL_0030;
						IL_0030:
						num2 = 4;
						y = num5 - tFirst;
						goto IL_003d;
						IL_003d:
						num2 = 5;
						num6 = 0.0;
						goto IL_004a;
						IL_004a:
						num2 = 6;
						num7 = 0.0;
						goto IL_0057;
						IL_0057:
						num2 = 7;
						vX *= Math.Pow(0.9976, y);
						goto IL_0076;
						IL_0076:
						num2 = 8;
						vY *= Math.Pow(0.9976, y);
						goto IL_0095;
						IL_0095:
						num2 = 9;
						if (vX > 0.0)
						{
							goto IL_00ac;
						}
						goto IL_01f7;
						IL_00ac:
						num2 = 10;
						num8 = (int)Math.Round(vX * num5 - 0.5 * num6 * Math.Pow(num5, 2.0));
						goto IL_00de;
						IL_00de:
						num2 = 11;
						num9 = (int)Math.Round(vX * tFirst - 0.5 * num6 * Math.Pow(tFirst, 2.0));
						goto IL_0118;
						IL_0118:
						num2 = 12;
						num10 = num8 - num9;
						goto IL_0122;
						IL_0122:
						num2 = 13;
						if (num10 <= 0)
						{
							goto IL_012a;
						}
						goto IL_0135;
						IL_012a:
						num2 = 14;
						num10 = 0;
						goto IL_0130;
						IL_0130:
						num2 = 15;
						flag = true;
						goto IL_0135;
						IL_0135:
						num2 = 16;
						if (cnvMainMenu.Margin.Left + (double)num10 + borderMainMenu.Margin.Left + borderMainMenu.Width > base.Width)
						{
							goto IL_017c;
						}
						goto IL_0327;
						IL_017c:
						num2 = 17;
						cnvMainMenu.Margin = new Thickness(base.Width - borderMainMenu.Margin.Left - borderMainMenu.Width, cnvMainMenu.Margin.Top, 0.0, 0.0);
						goto IL_01dc;
						IL_01dc:
						num2 = 18;
						num10 = 1;
						goto IL_01e2;
						IL_01e2:
						num2 = 19;
						vX = 0.0 - vX;
						goto IL_0327;
						IL_01f7:
						num2 = 21;
						num6 = 0.0 - num6;
						goto IL_01ff;
						IL_01ff:
						num2 = 22;
						num8 = (int)Math.Round(vX * num5 - 0.5 * num6 * Math.Pow(num5, 2.0));
						goto IL_0231;
						IL_0231:
						num2 = 23;
						num9 = (int)Math.Round(vX * tFirst - 0.5 * num6 * Math.Pow(tFirst, 2.0));
						goto IL_026b;
						IL_026b:
						num2 = 24;
						num10 = num8 - num9;
						goto IL_0275;
						IL_0275:
						num2 = 25;
						if (num10 >= 0)
						{
							goto IL_027d;
						}
						goto IL_0288;
						IL_027d:
						num2 = 26;
						num10 = 0;
						goto IL_0283;
						IL_0283:
						num2 = 27;
						flag = true;
						goto IL_0288;
						IL_0288:
						num2 = 28;
						if (cnvMainMenu.Margin.Left + (double)num10 + borderMainMenu.Margin.Left < 0.0)
						{
							goto IL_02c3;
						}
						goto IL_0327;
						IL_02c3:
						num2 = 29;
						cnvMainMenu.Margin = new Thickness(0.0 - borderMainMenu.Margin.Left, cnvMainMenu.Margin.Top, 0.0, 0.0);
						goto IL_0311;
						IL_0311:
						num2 = 30;
						num10 = 1;
						goto IL_0317;
						IL_0317:
						num2 = 31;
						vX = 0.0 - vX;
						goto IL_0327;
						IL_0327:
						num2 = 32;
						if (vY > 0.0)
						{
							goto IL_033e;
						}
						goto IL_048a;
						IL_033e:
						num2 = 33;
						num11 = (int)Math.Round(vY * num5 - 0.5 * num7 * Math.Pow(num5, 2.0));
						goto IL_0370;
						IL_0370:
						num2 = 34;
						num12 = (int)Math.Round(vY * tFirst - 0.5 * num7 * Math.Pow(tFirst, 2.0));
						goto IL_03aa;
						IL_03aa:
						num2 = 35;
						num13 = num11 - num12;
						goto IL_03b4;
						IL_03b4:
						num2 = 36;
						if (num13 <= 0)
						{
							goto IL_03bc;
						}
						goto IL_03c8;
						IL_03bc:
						num2 = 37;
						num13 = 0;
						goto IL_03c2;
						IL_03c2:
						num2 = 38;
						flag2 = true;
						goto IL_03c8;
						IL_03c8:
						num2 = 39;
						if (cnvMainMenu.Margin.Top + (double)num13 + borderMainMenu.Margin.Top + borderMainMenu.Height > base.Height)
						{
							goto IL_040f;
						}
						goto IL_05cf;
						IL_040f:
						num2 = 40;
						cnvMainMenu.Margin = new Thickness(cnvMainMenu.Margin.Left, base.Height - borderMainMenu.Margin.Top - borderMainMenu.Height, 0.0, 0.0);
						goto IL_046f;
						IL_046f:
						num2 = 41;
						num13 = 1;
						goto IL_0475;
						IL_0475:
						num2 = 42;
						vY = 0.0 - vY;
						goto IL_05cf;
						IL_048a:
						num2 = 44;
						num7 = 0.0 - num7;
						goto IL_0492;
						IL_0492:
						num2 = 45;
						num11 = (int)Math.Round(vY * num5 - 0.5 * num7 * Math.Pow(num5, 2.0));
						goto IL_04c4;
						IL_04c4:
						num2 = 46;
						num12 = (int)Math.Round(vY * tFirst - 0.5 * num7 * Math.Pow(tFirst, 2.0));
						goto IL_04fe;
						IL_04fe:
						num2 = 47;
						num13 = num11 - num12;
						goto IL_0508;
						IL_0508:
						num2 = 48;
						if (num13 >= 0)
						{
							goto IL_0510;
						}
						goto IL_051c;
						IL_0510:
						num2 = 49;
						num13 = 0;
						goto IL_0516;
						IL_0516:
						num2 = 50;
						flag2 = true;
						goto IL_051c;
						IL_051c:
						num2 = 51;
						if (cnvMainMenu.Margin.Top + (double)num13 + borderMainMenu.Margin.Top < 0.0)
						{
							goto IL_0557;
						}
						goto IL_05cf;
						IL_0557:
						num2 = 52;
						cnvMainMenu.Margin = new Thickness(cnvMainMenu.Margin.Left, cnvMainMenu.Margin.Top - borderMainMenu.Margin.Top, 0.0, 0.0);
						goto IL_05b9;
						IL_05b9:
						num2 = 53;
						num13 = 1;
						goto IL_05bf;
						IL_05bf:
						num2 = 54;
						vY = 0.0 - vY;
						goto IL_05cf;
						IL_05cf:
						num2 = 55;
						cnvMainMenu.Margin = new Thickness(cnvMainMenu.Margin.Left + (double)num10, cnvMainMenu.Margin.Top + (double)num13, 0.0, 0.0);
						goto IL_0624;
						IL_0624:
						num2 = 56;
						if (!unchecked(flag && flag2))
						{
							break;
						}
						goto IL_062d;
						IL_062d:
						num2 = 57;
						tmrShortGesture.IsEnabled = false;
						goto IL_063c;
						end_IL_0000_2:
						break;
					}
					num2 = 59;
					tFirst = num5;
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1882;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ActivateBackgroundPaper()
	{
		if (ShownSubMenu != null)
		{
			CollapseSubMenus();
		}
		Mouse.SetCursor(Cursors.Wait);
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			DefaultExt = ".png",
			Filter = "Resim Dosyaları|*.jpeg;*.jpg;*.png;*.gif;*.bmp;*.tif",
			Multiselect = false
		};
		if (Operators.CompareString(LastBrowsedPath, null, TextCompare: false) == 0)
		{
			openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Arka Plan Sayfaları";
		}
		else if (!LastBrowsedPath.Contains(AppDomain.CurrentDomain.BaseDirectory + "Arka Plan Sayfaları"))
		{
			openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Arka Plan Sayfaları";
		}
		Mouse.SetCursor(Cursors.Arrow);
		bool? flag = openFileDialog.ShowDialog();
		flag = flag;
		if (flag != true)
		{
			return;
		}
		LastBrowsedPath = openFileDialog.FileName;
		string fileName = openFileDialog.FileName;
		checked
		{
			try
			{
				cnvBackgroundPaper.Width = base.Width;
				cnvBackgroundPaper.Height = base.Height;
				Image image = new Image();
				cnvBackgroundPaper.Children.Add(image);
				image.Width = base.Width;
				image.Height = base.Height;
				RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
				image.Stretch = Stretch.Fill;
				image.Source = new BitmapImage(new Uri(fileName));
				cnvBackgroundPaper.Visibility = Visibility.Visible;
				AnimationFadeIn(image, 300);
				if (cnvCurtain.Visibility == Visibility.Visible)
				{
					RemoveCurtain();
				}
				FrameNo++;
				if (iRedo != 0)
				{
					ResetRedo();
				}
				if (FrameNo == 1)
				{
					SelectUndoImageOfFav();
				}
				iAddedBackgroundPapers++;
				FrameNoOfBackgroundPaperAdded = (int[])Utils.CopyArray(FrameNoOfBackgroundPaperAdded, new int[iAddedBackgroundPapers + 1]);
				FrameNoOfBackgroundPaperAdded[iAddedBackgroundPapers] = (int)FrameNo;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}
	}

	private void ActivateEraser(int NewWidth, int NewHeight, bool isGestureEraser = false)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 245:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001d;
						case 5:
							goto IL_0025;
						case 6:
							goto IL_004a;
						case 7:
							goto IL_0053;
						case 8:
							goto IL_005d;
						case 9:
							goto IL_0065;
						case 10:
							goto IL_006e;
						case 11:
							goto IL_008e;
						case 12:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 13:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_008e:
					num2 = 11;
					inkCanvas.EditingMode = InkCanvasEditingMode.None;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (DrawState == DrawStateEnum.NoPen)
					{
						goto IL_001d;
					}
					goto IL_004a;
					IL_001d:
					num2 = 4;
					ActivatePen();
					goto IL_0025;
					IL_0025:
					num2 = 5;
					AnimationFadeIn(imgPen, 1500);
					goto IL_004a;
					IL_004a:
					num2 = 6;
					DrawState = DrawStateEnum.Eraser;
					goto IL_0053;
					IL_0053:
					num2 = 7;
					if (ShownSubMenu != null)
					{
						goto IL_005d;
					}
					goto IL_0065;
					IL_005d:
					num2 = 8;
					CollapseSubMenus();
					goto IL_0065;
					IL_0065:
					num2 = 9;
					ChangeTickOfMode();
					goto IL_006e;
					IL_006e:
					num2 = 10;
					inkCanvas.EraserShape = new RectangleStylusShape(NewWidth, NewHeight, 170.0);
					goto IL_008e;
					end_IL_0000_2:
					break;
				}
				num2 = 12;
				inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 245;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ActivatePen()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 213:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001a;
						case 5:
							goto IL_0032;
						case 6:
							goto IL_003b;
						case 7:
							goto IL_0049;
						case 8:
							goto IL_0064;
						case 9:
							goto IL_0072;
						case 10:
							goto IL_007b;
						case 11:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 12:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_007b:
					num2 = 10;
					ChangeTickOfMode();
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					DrawState = DrawStateEnum.Pen;
					goto IL_001a;
					IL_001a:
					num2 = 4;
					imgPen.Tag = DrawState;
					goto IL_0032;
					IL_0032:
					num2 = 5;
					ChangeInkSize(0);
					goto IL_003b;
					IL_003b:
					num2 = 6;
					inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
					goto IL_0049;
					IL_0049:
					num2 = 7;
					inkCanvas.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
					goto IL_0064;
					IL_0064:
					num2 = 8;
					inkCanvas.IsEnabled = true;
					goto IL_0072;
					IL_0072:
					num2 = 9;
					ChangePenImage();
					goto IL_007b;
					end_IL_0000_2:
					break;
				}
				num2 = 11;
				DrawStateBeforeAction = DrawState;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 213;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ActivateShape(DrawStateEnum NewShape)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				DrawStateEnum drawState;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 194:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001a;
						case 6:
							goto IL_002b;
						case 8:
							goto IL_003b;
						case 5:
						case 7:
						case 9:
						case 10:
							goto IL_0049;
						case 11:
							goto IL_0058;
						case 12:
							goto IL_0063;
						case 13:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 14:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0058:
					num2 = 11;
					if (ShownSubMenu == null)
					{
						break;
					}
					goto IL_0063;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					DrawState = NewShape;
					goto IL_001a;
					IL_001a:
					num2 = 4;
					drawState = DrawState;
					if (drawState == DrawStateEnum.DashLine)
					{
						goto IL_003b;
					}
					if (drawState == DrawStateEnum.Triangle)
					{
						goto IL_002b;
					}
					goto IL_0049;
					IL_0063:
					num2 = 12;
					CollapseSubMenus();
					break;
					IL_002b:
					num2 = 6;
					TrianglePoints = new Point[4];
					goto IL_0049;
					IL_003b:
					num2 = 8;
					strokeDash = new Stroke[1];
					goto IL_0049;
					IL_0049:
					num2 = 10;
					inkCanvas.EditingMode = InkCanvasEditingMode.None;
					goto IL_0058;
					end_IL_0000_2:
					break;
				}
				num2 = 13;
				ChangeTickOfMode();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 194;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ClearCanvas()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		bool flag = default(bool);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								int[] frameNoOfLibraryCollapsed = null;
					int[] frameNoOfBackgroundCollapsed = null;
					int[] frameNoOfErase = null;
					StrokeCollection[] undoStrokeCollectionStack = null;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 816:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_0015;
							case 5:
								goto IL_0020;
							case 6:
								goto IL_0028;
							case 7:
								goto IL_0037;
							case 8:
								goto IL_0045;
							case 9:
								goto IL_0057;
							case 10:
								goto IL_0067;
							case 11:
								goto IL_0076;
							case 12:
								goto IL_0087;
							case 13:
								goto IL_00ae;
							case 14:
								goto IL_00c8;
							case 15:
								goto IL_00cd;
							case 16:
								goto IL_00dd;
							case 17:
								goto IL_00ec;
							case 18:
								goto IL_00fd;
							case 19:
								goto IL_0124;
							case 20:
								goto IL_013e;
							case 21:
								goto IL_0143;
							case 22:
								goto IL_0153;
							case 23:
								goto IL_015c;
							case 24:
								goto IL_016e;
							case 25:
								goto IL_0173;
							case 26:
								goto IL_018c;
							case 27:
								goto IL_019d;
							case 28:
								goto IL_01c4;
							case 29:
								goto IL_01de;
							case 30:
								goto IL_0205;
							case 31:
								goto IL_0225;
							case 32:
								goto IL_0238;
							case 33:
								goto IL_023d;
							case 34:
								goto IL_0243;
							case 35:
								goto IL_0255;
							case 36:
								goto IL_0260;
							case 37:
								goto IL_0269;
							case 38:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 39:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_0269:
						num2 = 37;
						if (FrameNo != 1)
						{
							goto end_IL_0000_3;
						}
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						flag = false;
						goto IL_0015;
						IL_0015:
						num2 = 4;
						if (DrawState == DrawStateEnum.Eraser)
						{
							goto IL_0020;
						}
						goto IL_0028;
						IL_0020:
						num2 = 5;
						ActivatePen();
						goto IL_0028;
						IL_0028:
						num2 = 6;
						if (cnvLibrary.Visibility == Visibility.Visible)
						{
							goto IL_0037;
						}
						goto IL_0057;
						IL_0037:
						num2 = 7;
						cnvLibrary.Visibility = Visibility.Collapsed;
						goto IL_0045;
						IL_0045:
						num2 = 8;
						isGestureEnabled = MySettingsProperty.Settings.isGestureEnabled;
						goto IL_0057;
						IL_0057:
						num2 = 9;
						if (cnvLibraryBack.Visibility == Visibility.Visible)
						{
							goto IL_0067;
						}
						goto IL_00cd;
						IL_0067:
						num2 = 10;
						cnvLibraryBack.Visibility = Visibility.Collapsed;
						goto IL_0076;
						IL_0076:
						num2 = 11;
						iCollapsedLibraries++;
						goto IL_0087;
						IL_0087:
						num2 = 12;
						frameNoOfLibraryCollapsed = FrameNoOfLibraryCollapsed;
						frameNoOfLibraryCollapsed = (int[])Utils.CopyArray(frameNoOfLibraryCollapsed, new int[iCollapsedLibraries + 1]);
						FrameNoOfLibraryCollapsed = frameNoOfLibraryCollapsed;
						goto IL_00ae;
						IL_00ae:
						num2 = 13;
						FrameNoOfLibraryCollapsed[iCollapsedLibraries] = (int)(FrameNo + 1);
						goto IL_00c8;
						IL_00c8:
						num2 = 14;
						flag = true;
						goto IL_00cd;
						IL_00cd:
						num2 = 15;
						if (cnvBackgroundPaper.Visibility == Visibility.Visible)
						{
							goto IL_00dd;
						}
						goto IL_0143;
						IL_00dd:
						num2 = 16;
						cnvBackgroundPaper.Visibility = Visibility.Collapsed;
						goto IL_00ec;
						IL_00ec:
						num2 = 17;
						iCollapsedBackgrounds++;
						goto IL_00fd;
						IL_00fd:
						num2 = 18;
						frameNoOfBackgroundCollapsed = FrameNoOfBackgroundCollapsed;
						frameNoOfBackgroundCollapsed = (int[])Utils.CopyArray(frameNoOfBackgroundCollapsed, new int[iCollapsedBackgrounds + 1]);
						FrameNoOfBackgroundCollapsed = frameNoOfBackgroundCollapsed;
						goto IL_0124;
						IL_0124:
						num2 = 19;
						FrameNoOfBackgroundCollapsed[iCollapsedBackgrounds] = (int)(FrameNo + 1);
						goto IL_013e;
						IL_013e:
						num2 = 20;
						flag = true;
						goto IL_0143;
						IL_0143:
						num2 = 21;
						if (cnvCurtain.Visibility == Visibility.Visible)
						{
							goto IL_0153;
						}
						goto IL_0173;
						IL_0153:
						num2 = 22;
						RemoveCurtain();
						goto IL_015c;
						IL_015c:
						num2 = 23;
						FrameNo--;
						goto IL_016e;
						IL_016e:
						num2 = 24;
						flag = true;
						goto IL_0173;
						IL_0173:
						num2 = 25;
						if (inkCanvas.Strokes.Count > 0)
						{
							goto IL_018c;
						}
						goto IL_023d;
						IL_018c:
						num2 = 26;
						iErase++;
						goto IL_019d;
						IL_019d:
						num2 = 27;
						frameNoOfErase = FrameNoOfErase;
						frameNoOfErase = (int[])Utils.CopyArray(frameNoOfErase, new int[iErase + 1]);
						FrameNoOfErase = frameNoOfErase;
						goto IL_01c4;
						IL_01c4:
						num2 = 28;
						FrameNoOfErase[iErase] = (int)(FrameNo + 1);
						goto IL_01de;
						IL_01de:
						num2 = 29;
						undoStrokeCollectionStack = UndoStrokeCollectionStack;
						undoStrokeCollectionStack = (StrokeCollection[])Utils.CopyArray(undoStrokeCollectionStack, new StrokeCollection[iErase + 1]);
						UndoStrokeCollectionStack = undoStrokeCollectionStack;
						goto IL_0205;
						IL_0205:
						num2 = 30;
						UndoStrokeCollectionStack[iErase] = inkCanvas.Strokes.Clone();
						goto IL_0225;
						IL_0225:
						num2 = 31;
						inkCanvas.Strokes.Clear();
						goto IL_0238;
						IL_0238:
						num2 = 32;
						flag = true;
						goto IL_023d;
						IL_023d:
						num2 = 33;
						if (!flag)
						{
							goto end_IL_0000_3;
						}
						goto IL_0243;
						IL_0243:
						num2 = 34;
						FrameNo++;
						goto IL_0255;
						IL_0255:
						num2 = 35;
						if (iRedo != 0)
						{
							goto IL_0260;
						}
						goto IL_0269;
						IL_0260:
						num2 = 36;
						ResetRedo();
						goto IL_0269;
						end_IL_0000_2:
						break;
					}
					num2 = 38;
					SelectUndoImageOfFav();
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 816;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void CollapseSubMenus()
	{
		if (ShownSubMenu != null)
		{
			ShownSubMenu.Visibility = Visibility.Collapsed;
			ShownSubMenu = null;
		}
	}

	private void CancelDrawingOfTriangle()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 102:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001a;
						case 5:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 6:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_001a:
					num2 = 4;
					inkCanvas.Strokes.Remove(st);
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					isTrianglesSecondStep = false;
					goto IL_001a;
					end_IL_0000_2:
					break;
				}
				num2 = 5;
				st = null;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 102;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
	private void CheckPrevinstanceAndCommandLineArguments()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		string[] commandLineArgs = default(string[]);
		int num5 = default(int);
		int num6 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 156:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_002e;
							case 5:
								goto IL_0036;
							case 6:
								goto IL_0044;
							case 7:
								goto IL_005c;
							default:
								goto end_IL_0000;
							case 8:
								goto end_IL_0000_2;
							}
							goto default;
						}
						IL_0044:
						num2 = 6;
						if (Operators.CompareString(commandLineArgs[num5], "/PreLoad", TextCompare: false) == 0)
						{
							ProjectData.EndApp();
						}
						goto IL_005c;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length - 1 > 0)
						{
							ProjectData.EndApp();
						}
						goto IL_002e;
						IL_005c:
						num2 = 7;
						num5++;
						goto IL_0064;
						IL_002e:
						num2 = 4;
						commandLineArgs = Environment.GetCommandLineArgs();
						goto IL_0036;
						IL_0036:
						num2 = 5;
						num6 = commandLineArgs.Length - 1;
						num5 = 0;
						goto IL_0064;
						IL_0064:
						if (num5 > num6)
						{
							goto end_IL_0000_2;
						}
						goto IL_0044;
						end_IL_0000:
						break;
					}
							}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 156;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_2:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void CloseMe(object sender, MouseButtonEventArgs e)
	{
		AnimationFadeOut(this, 200);
	}

	private void SetWindowSize()
	{
		if ((base.Left != SystemParameters.WorkArea.Left) | (base.Top != SystemParameters.WorkArea.Top) | (base.Width != SystemParameters.WorkArea.Width) | (base.Height != SystemParameters.WorkArea.Height))
		{
			base.Left = SystemParameters.WorkArea.Left;
			base.Top = SystemParameters.WorkArea.Top;
			base.Width = SystemParameters.WorkArea.Width;
			base.Height = SystemParameters.WorkArea.Height;
			if (isSideArrowsEnabled)
			{
				borderSideArrowRight.Margin = new Thickness(base.Width - imgSideArrowRight.Width, base.Height - imgSideArrowRight.Height, 0.0, 0.0);
				AnimationFadeIn(borderSideArrowRight, 500, borderSideArrowRight.Opacity);
				borderSideArrowLeft.Margin = new Thickness(0.0, base.Height - imgSideArrowLeft.Height, 0.0, 0.0);
				AnimationFadeIn(borderSideArrowLeft, 500, borderSideArrowLeft.Opacity);
			}
			inkCanvas.Width = base.Width;
			inkCanvas.Height = base.Height;
			rectFrame.Width = base.Width;
			rectFrame.Height = base.Height;
		}
	}

	private void InitTimers()
	{
		tmrVelocity = new DispatcherTimer
		{
			Interval = new TimeSpan(0, 0, 0, 0, 100)
		};
		tmrVelocity.Tick += tmrVelocity_Tick;
		tmrAcceleration = new DispatcherTimer
		{
			Interval = new TimeSpan(0, 0, 0, 0, 1)
		};
		tmrAcceleration.Tick += tmrAcceleration_Tick;
		tmrSideArrows = new DispatcherTimer
		{
			Interval = new TimeSpan(0, 0, 0, 0, 1)
		};
		tmrSideArrows.Tick += tmrSideArrows_Tick;
		tmrShortGesture = new DispatcherTimer
		{
			Interval = new TimeSpan(0, 0, 0, 0, 1)
		};
		tmrShortGesture.Tick += tmrShortGesture_Tick;
		tmrReShowSideArrowRight = new DispatcherTimer
		{
			Interval = new TimeSpan(0, 0, 0, 5, 0)
		};
		tmrReShowSideArrowRight.Tick += tmrReShowSideArrowRight_Tick;
		tmrReShowSideArrowLeft = new DispatcherTimer
		{
			Interval = new TimeSpan(0, 0, 0, 5, 0)
		};
		tmrReShowSideArrowLeft.Tick += tmrReShowSideArrowLeft_Tick;
	}

	private void AnimationFadeIn(object AniObject, int AniDuration, double AniFrom = 0.0, double AniTo = 1.0)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		DoubleAnimation doubleAnimation = default(DoubleAnimation);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 301:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_0030;
						case 5:
							goto IL_004f;
						case 6:
							goto IL_0082;
						case 7:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 8:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0082:
					num2 = 6;
					NewLateBinding.LateCall(AniObject, null, "BeginAnimation", new object[2]
					{
						UIElement.OpacityProperty,
						null
					}, null, null, null, IgnoreReturn: true);
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					NewLateBinding.LateSet(AniObject, null, "Opacity", new object[1] { 0 }, null, null);
					goto IL_0030;
					IL_0030:
					num2 = 4;
					NewLateBinding.LateSet(AniObject, null, "Visibility", new object[1] { Visibility.Visible }, null, null);
					goto IL_004f;
					IL_004f:
					num2 = 5;
					doubleAnimation = new DoubleAnimation
					{
						From = AniFrom,
						To = AniTo,
						Duration = new Duration(TimeSpan.FromMilliseconds(AniDuration))
					};
					goto IL_0082;
					end_IL_0000_2:
					break;
				}
				num2 = 7;
				object[] obj = new object[2]
				{
					UIElement.OpacityProperty,
					doubleAnimation
				};
				object[] array = obj;
				bool[] obj2 = new bool[2] { false, true };
				bool[] array2 = obj2;
				NewLateBinding.LateCall(AniObject, null, "BeginAnimation", obj, null, null, obj2, IgnoreReturn: true);
				if (array2[1])
				{
					doubleAnimation = (DoubleAnimation)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[1]), typeof(DoubleAnimation));
				}
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 301;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void AnimationFadeOut(object AniObject, int AniDuration, double AniFrom = 1.0, double AniTo = 0.0)
	{
		DoubleAnimation doubleAnimation = new DoubleAnimation
		{
			From = AniFrom,
			To = AniTo,
			Duration = new Duration(TimeSpan.FromMilliseconds(AniDuration))
		};
		FadeOutObject = RuntimeHelpers.GetObjectValue(AniObject);
		doubleAnimation.Completed += [SpecialName] (object a0, EventArgs a1) =>
		{
			AnimationFadeOutEnded();
		};
		NewLateBinding.LateCall(AniObject, null, "BeginAnimation", new object[2]
		{
			UIElement.OpacityProperty,
			null
		}, null, null, null, IgnoreReturn: true);
		object[] obj = new object[2]
		{
			UIElement.OpacityProperty,
			doubleAnimation
		};
		object[] array = obj;
		bool[] obj2 = new bool[2] { false, true };
		bool[] array2 = obj2;
		NewLateBinding.LateCall(AniObject, null, "BeginAnimation", obj, null, null, obj2, IgnoreReturn: true);
		if (array2[1])
		{
			doubleAnimation = (DoubleAnimation)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[1]), typeof(DoubleAnimation));
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
	private void AnimationFadeOutEnded()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		string left = default(string);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 255:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 5:
							goto IL_001f;
						case 6:
							goto IL_0030;
						case 8:
							goto IL_003f;
						case 9:
							goto IL_0055;
						case 11:
							goto IL_0066;
						case 12:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 4:
						case 7:
						case 10:
						case 13:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0066:
					num2 = 11;
					if (!Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(FadeOutObject, null, "opacity", new object[0], null, null, null), 0, TextCompare: false))
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					left = FadeOutObject.ToString();
					goto IL_001f;
					IL_001f:
					num2 = 5;
					if (Operators.CompareString(left, ToString(), TextCompare: false) == 0)
					{
						goto IL_0030;
					}
					goto IL_003f;
					IL_0030:
					num2 = 6;
					Close();
					ProjectData.EndApp();
					goto end_IL_0000_3;
					IL_003f:
					num2 = 8;
					if (Operators.CompareString(left, cnvLibrary.ToString(), TextCompare: false) == 0)
					{
						goto IL_0055;
					}
					goto IL_0066;
					IL_0055:
					num2 = 9;
					cnvLibrary.Visibility = Visibility.Collapsed;
					goto end_IL_0000_3;
					end_IL_0000_2:
					break;
				}
				num2 = 12;
				NewLateBinding.LateSet(FadeOutObject, null, "Visibility", new object[1] { Visibility.Collapsed }, null, null);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 255;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ChangeColor(byte NewColorNo)
	{
		ColorNo = NewColorNo;
		switch (ColorNo)
		{
		case 1:
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				InkColor = Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, 0);
				break;
			case PenStyleEnum.Stylograph:
				InkColor = Color.FromArgb(byte.MaxValue, 230, 27, 27);
				break;
			case PenStyleEnum.Highlighter:
				InkColor = Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				break;
			}
			break;
		case 2:
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				InkColor = Color.FromArgb(byte.MaxValue, 0, 0, byte.MaxValue);
				break;
			case PenStyleEnum.Stylograph:
				InkColor = Color.FromArgb(byte.MaxValue, 0, 0, 157);
				break;
			case PenStyleEnum.Highlighter:
				InkColor = Color.FromArgb(byte.MaxValue, 1, byte.MaxValue, byte.MaxValue);
				break;
			}
			break;
		case 3:
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				InkColor = Color.FromArgb(byte.MaxValue, 0, 110, 0);
				break;
			case PenStyleEnum.Stylograph:
				InkColor = Color.FromArgb(byte.MaxValue, 0, 110, 0);
				break;
			case PenStyleEnum.Highlighter:
				InkColor = Color.FromArgb(byte.MaxValue, 0, byte.MaxValue, 1);
				break;
			}
			break;
		case 4:
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				InkColor = Color.FromArgb(byte.MaxValue, byte.MaxValue, 115, 0);
				break;
			case PenStyleEnum.Stylograph:
				InkColor = Color.FromArgb(byte.MaxValue, byte.MaxValue, 152, 0);
				break;
			case PenStyleEnum.Highlighter:
				InkColor = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
				break;
			}
			break;
		case 5:
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				InkColor = Colors.Black;
				break;
			case PenStyleEnum.Stylograph:
				InkColor = Color.FromArgb(byte.MaxValue, 20, 20, 20);
				break;
			case PenStyleEnum.Highlighter:
				InkColor = Colors.Black;
				break;
			}
			break;
		case 6:
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				InkColor = Colors.White;
				break;
			case PenStyleEnum.Stylograph:
				InkColor = Colors.White;
				break;
			case PenStyleEnum.Highlighter:
				InkColor = Colors.White;
				break;
			}
			break;
		}
		inkCanvas.DefaultDrawingAttributes.Color = InkColor;
	}

	private void ChangeInkSize(byte NewInkSize = 0)
	{
		if (NewInkSize > 0)
		{
			InkSize = NewInkSize;
		}
		switch (InkSize)
		{
		case 1:
			ShapeSize = 1;
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				inkCanvas.DefaultDrawingAttributes.Width = 1.0;
				inkCanvas.DefaultDrawingAttributes.Height = 1.0;
				break;
			case PenStyleEnum.Highlighter:
				inkCanvas.DefaultDrawingAttributes.Width = 5.0;
				inkCanvas.DefaultDrawingAttributes.Height = 10.0;
				break;
			case PenStyleEnum.Stylograph:
				inkCanvas.DefaultDrawingAttributes.Width = 0.75;
				inkCanvas.DefaultDrawingAttributes.Height = 2.25;
				break;
			}
			break;
		case 2:
			ShapeSize = 2;
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				inkCanvas.DefaultDrawingAttributes.Width = 2.0;
				inkCanvas.DefaultDrawingAttributes.Height = 2.0;
				break;
			case PenStyleEnum.Highlighter:
				inkCanvas.DefaultDrawingAttributes.Width = 8.0;
				inkCanvas.DefaultDrawingAttributes.Height = 16.0;
				break;
			case PenStyleEnum.Stylograph:
				inkCanvas.DefaultDrawingAttributes.Width = 1.25;
				inkCanvas.DefaultDrawingAttributes.Height = 3.75;
				break;
			}
			break;
		case 3:
			ShapeSize = 3;
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				inkCanvas.DefaultDrawingAttributes.Width = 3.0;
				inkCanvas.DefaultDrawingAttributes.Height = 3.0;
				break;
			case PenStyleEnum.Highlighter:
				inkCanvas.DefaultDrawingAttributes.Width = 11.0;
				inkCanvas.DefaultDrawingAttributes.Height = 22.0;
				break;
			case PenStyleEnum.Stylograph:
				inkCanvas.DefaultDrawingAttributes.Width = 1.75;
				inkCanvas.DefaultDrawingAttributes.Height = 6.0;
				break;
			}
			break;
		case 4:
			ShapeSize = 5;
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				inkCanvas.DefaultDrawingAttributes.Width = 5.0;
				inkCanvas.DefaultDrawingAttributes.Height = 5.0;
				break;
			case PenStyleEnum.Highlighter:
				inkCanvas.DefaultDrawingAttributes.Width = 14.0;
				inkCanvas.DefaultDrawingAttributes.Height = 28.0;
				break;
			case PenStyleEnum.Stylograph:
				inkCanvas.DefaultDrawingAttributes.Width = 2.25;
				inkCanvas.DefaultDrawingAttributes.Height = 9.0;
				break;
			}
			break;
		case 5:
			ShapeSize = 10;
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				inkCanvas.DefaultDrawingAttributes.Width = 10.0;
				inkCanvas.DefaultDrawingAttributes.Height = 10.0;
				break;
			case PenStyleEnum.Highlighter:
				inkCanvas.DefaultDrawingAttributes.Width = 20.0;
				inkCanvas.DefaultDrawingAttributes.Height = 40.0;
				break;
			case PenStyleEnum.Stylograph:
				inkCanvas.DefaultDrawingAttributes.Width = 5.0;
				inkCanvas.DefaultDrawingAttributes.Height = 20.0;
				break;
			}
			break;
		case 6:
			ShapeSize = 22;
			switch (PenStyle)
			{
			case PenStyleEnum.Marker:
				inkCanvas.DefaultDrawingAttributes.Width = 22.0;
				inkCanvas.DefaultDrawingAttributes.Height = 22.0;
				break;
			case PenStyleEnum.Highlighter:
				inkCanvas.DefaultDrawingAttributes.Width = 30.0;
				inkCanvas.DefaultDrawingAttributes.Height = 60.0;
				break;
			case PenStyleEnum.Stylograph:
				inkCanvas.DefaultDrawingAttributes.Width = 8.0;
				inkCanvas.DefaultDrawingAttributes.Height = 35.0;
				break;
			}
			break;
		}
	}

	private void ChangeInkSizeFrame()
	{
		imgSize1SelectionR.Visibility = Visibility.Collapsed;
		imgSize2SelectionR.Visibility = Visibility.Collapsed;
		imgSize3SelectionR.Visibility = Visibility.Collapsed;
		imgSize4SelectionR.Visibility = Visibility.Collapsed;
		imgSize5SelectionR.Visibility = Visibility.Collapsed;
		imgSize6SelectionR.Visibility = Visibility.Collapsed;
		imgSize1SelectionL.Visibility = Visibility.Collapsed;
		imgSize2SelectionL.Visibility = Visibility.Collapsed;
		imgSize3SelectionL.Visibility = Visibility.Collapsed;
		imgSize4SelectionL.Visibility = Visibility.Collapsed;
		imgSize5SelectionL.Visibility = Visibility.Collapsed;
		imgSize6SelectionL.Visibility = Visibility.Collapsed;
		switch (InkSize)
		{
		case 1:
			imgSize1SelectionR.Visibility = Visibility.Visible;
			imgSize1SelectionL.Visibility = Visibility.Visible;
			break;
		case 2:
			imgSize2SelectionR.Visibility = Visibility.Visible;
			imgSize2SelectionL.Visibility = Visibility.Visible;
			break;
		case 3:
			imgSize3SelectionR.Visibility = Visibility.Visible;
			imgSize3SelectionL.Visibility = Visibility.Visible;
			break;
		case 4:
			imgSize4SelectionR.Visibility = Visibility.Visible;
			imgSize4SelectionL.Visibility = Visibility.Visible;
			break;
		case 5:
			imgSize5SelectionR.Visibility = Visibility.Visible;
			imgSize5SelectionL.Visibility = Visibility.Visible;
			break;
		case 6:
			imgSize6SelectionR.Visibility = Visibility.Visible;
			imgSize6SelectionL.Visibility = Visibility.Visible;
			break;
		}
	}

	private void ChangePenImage()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Matrix stylusTipTransform = default(Matrix);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 1323:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 5:
							goto IL_0031;
						case 6:
							goto IL_0044;
						case 7:
							goto IL_0057;
						case 8:
							goto IL_0073;
						case 10:
							goto IL_00a3;
						case 12:
							goto IL_00c1;
						case 14:
							goto IL_00df;
						case 16:
							goto IL_00fd;
						case 18:
							goto IL_011b;
						case 20:
							goto IL_0139;
						case 22:
							goto IL_0157;
						case 25:
							goto IL_0175;
						case 26:
							goto IL_0189;
						case 27:
							goto IL_019d;
						case 28:
							goto IL_01a8;
						case 29:
							goto IL_01bb;
						case 30:
							goto IL_01d0;
						case 32:
							goto IL_0201;
						case 34:
							goto IL_021f;
						case 36:
							goto IL_023d;
						case 38:
							goto IL_025b;
						case 40:
							goto IL_0279;
						case 42:
							goto IL_0297;
						case 44:
							goto IL_02b5;
						case 47:
							goto IL_02d3;
						case 48:
							goto IL_02e7;
						case 49:
							goto IL_02fb;
						case 50:
							goto IL_0318;
						case 52:
							goto IL_0349;
						case 54:
							goto IL_0367;
						case 56:
							goto IL_0385;
						case 58:
							goto IL_03a0;
						case 60:
							goto IL_03bb;
						case 62:
							goto IL_03d6;
						case 64:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 4:
						case 9:
						case 11:
						case 13:
						case 15:
						case 17:
						case 19:
						case 21:
						case 23:
						case 24:
						case 31:
						case 33:
						case 35:
						case 37:
						case 39:
						case 41:
						case 43:
						case 45:
						case 46:
						case 51:
						case 53:
						case 55:
						case 57:
						case 59:
						case 61:
						case 63:
						case 65:
						case 66:
						case 67:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00a3:
					num2 = 10;
					imgPen.Source = imgPenRed.Source;
					goto end_IL_0000_3;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					switch (PenStyle)
					{
					case PenStyleEnum.Marker:
						break;
					case PenStyleEnum.Stylograph:
						goto IL_0175;
					case PenStyleEnum.Highlighter:
						goto IL_02d3;
					default:
						goto end_IL_0000_3;
					}
					goto IL_0031;
					IL_02d3:
					num2 = 47;
					inkCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Rectangle;
					goto IL_02e7;
					IL_02e7:
					num2 = 48;
					inkCanvas.DefaultDrawingAttributes.IsHighlighter = true;
					goto IL_02fb;
					IL_02fb:
					num2 = 49;
					inkCanvas.DefaultDrawingAttributes.StylusTipTransform = default(Matrix);
					goto IL_0318;
					IL_0318:
					num2 = 50;
					switch (ColorNo)
					{
					case 1:
						break;
					case 2:
						goto IL_0367;
					case 3:
						goto IL_0385;
					case 4:
						goto IL_03a0;
					case 5:
						goto IL_03bb;
					case 6:
						goto IL_03d6;
					default:
						goto end_IL_0000_2;
					}
					goto IL_0349;
					IL_03d6:
					num2 = 62;
					imgPen.Source = imgHighlighterWhite.Source;
					goto end_IL_0000_3;
					IL_03bb:
					num2 = 60;
					imgPen.Source = imgHighlighterBlack.Source;
					goto end_IL_0000_3;
					IL_03a0:
					num2 = 58;
					imgPen.Source = imgHighlighterOrange.Source;
					goto end_IL_0000_3;
					IL_0385:
					num2 = 56;
					imgPen.Source = imgHighlighterGreen.Source;
					goto end_IL_0000_3;
					IL_0367:
					num2 = 54;
					imgPen.Source = imgHighlighterBlue.Source;
					goto end_IL_0000_3;
					IL_0349:
					num2 = 52;
					imgPen.Source = imgHighlighterRed.Source;
					goto end_IL_0000_3;
					IL_0175:
					num2 = 25;
					inkCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
					goto IL_0189;
					IL_0189:
					num2 = 26;
					inkCanvas.DefaultDrawingAttributes.IsHighlighter = false;
					goto IL_019d;
					IL_019d:
					num2 = 27;
					stylusTipTransform = default(Matrix);
					goto IL_01a8;
					IL_01a8:
					num2 = 28;
					stylusTipTransform.Rotate(45.0);
					goto IL_01bb;
					IL_01bb:
					num2 = 29;
					inkCanvas.DefaultDrawingAttributes.StylusTipTransform = stylusTipTransform;
					goto IL_01d0;
					IL_01d0:
					num2 = 30;
					switch (ColorNo)
					{
					case 1:
						break;
					case 2:
						goto IL_021f;
					case 3:
						goto IL_023d;
					case 4:
						goto IL_025b;
					case 5:
						goto IL_0279;
					case 6:
						goto IL_0297;
					default:
						goto IL_02b5;
					}
					goto IL_0201;
					IL_02b5:
					num2 = 44;
					imgPen.Source = imgStylographColorful.Source;
					goto end_IL_0000_3;
					IL_0297:
					num2 = 42;
					imgPen.Source = imgStylographWhite.Source;
					goto end_IL_0000_3;
					IL_0279:
					num2 = 40;
					imgPen.Source = imgStylographBlack.Source;
					goto end_IL_0000_3;
					IL_025b:
					num2 = 38;
					imgPen.Source = imgStylographOrange.Source;
					goto end_IL_0000_3;
					IL_023d:
					num2 = 36;
					imgPen.Source = imgStylographGreen.Source;
					goto end_IL_0000_3;
					IL_021f:
					num2 = 34;
					imgPen.Source = imgStylographBlue.Source;
					goto end_IL_0000_3;
					IL_0201:
					num2 = 32;
					imgPen.Source = imgStylographRed.Source;
					goto end_IL_0000_3;
					IL_0031:
					num2 = 5;
					inkCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
					goto IL_0044;
					IL_0044:
					num2 = 6;
					inkCanvas.DefaultDrawingAttributes.IsHighlighter = false;
					goto IL_0057;
					IL_0057:
					num2 = 7;
					inkCanvas.DefaultDrawingAttributes.StylusTipTransform = default(Matrix);
					goto IL_0073;
					IL_0073:
					num2 = 8;
					switch (ColorNo)
					{
					case 1:
						break;
					case 2:
						goto IL_00c1;
					case 3:
						goto IL_00df;
					case 4:
						goto IL_00fd;
					case 5:
						goto IL_011b;
					case 6:
						goto IL_0139;
					default:
						goto IL_0157;
					}
					goto IL_00a3;
					IL_0157:
					num2 = 22;
					imgPen.Source = imgPenColorful.Source;
					goto end_IL_0000_3;
					IL_0139:
					num2 = 20;
					imgPen.Source = imgPenWhite.Source;
					goto end_IL_0000_3;
					IL_011b:
					num2 = 18;
					imgPen.Source = imgPenBlack.Source;
					goto end_IL_0000_3;
					IL_00fd:
					num2 = 16;
					imgPen.Source = imgPenOrange.Source;
					goto end_IL_0000_3;
					IL_00df:
					num2 = 14;
					imgPen.Source = imgPenGreen.Source;
					goto end_IL_0000_3;
					IL_00c1:
					num2 = 12;
					imgPen.Source = imgPenBlue.Source;
					goto end_IL_0000_3;
					end_IL_0000_2:
					break;
				}
				num2 = 64;
				imgPen.Source = imgHighlighterColorful.Source;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1323;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ChangeTickOfMode()
	{
		switch (DrawState)
		{
		case DrawStateEnum.Pen:
			imgTickOfPen.Visibility = Visibility.Visible;
			AnimationFadeIn(imgTickOfPen, 300);
			imgTickOfEraser.Visibility = Visibility.Collapsed;
			imgTickOfShape.Visibility = Visibility.Collapsed;
			break;
		case DrawStateEnum.Eraser:
			imgTickOfPen.Visibility = Visibility.Collapsed;
			imgTickOfEraser.Visibility = Visibility.Visible;
			AnimationFadeIn(imgTickOfEraser, 300);
			imgTickOfShape.Visibility = Visibility.Collapsed;
			break;
		default:
			imgTickOfPen.Visibility = Visibility.Collapsed;
			imgTickOfEraser.Visibility = Visibility.Collapsed;
			imgTickOfShape.Visibility = Visibility.Visible;
			AnimationFadeIn(imgTickOfShape, 300);
			break;
		}
	}

	private void ShowColorSelector()
	{
		ShownSubMenu = GridColorSelector;
		if (!isLeftSubMenu)
		{
			GridColorSelector.Margin = new Thickness(223.0, 19.0, 0.0, 0.0);
		}
		else
		{
			GridColorSelector.Margin = new Thickness(15.0, 19.0, 0.0, 0.0);
		}
		AnimationFadeIn(GridColorSelector, 200);
	}

	private void SelectColorFromCartela(byte Red, byte Green, byte Blue)
	{
		InkColor = Color.FromArgb(byte.MaxValue, Red, Green, Blue);
		PenStyle = ActiveTab;
		ColorNo = 7;
		inkCanvas.DefaultDrawingAttributes.Color = InkColor;
		ActivatePen();
		if (ShownSubMenu != null)
		{
			CollapseSubMenus();
		}
	}

	private void UndoIt()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		isUndoneProp isUndoneProp = default(isUndoneProp);
		int num7 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								isUndoneProp[] redo = null;
					int num5;
					int num6;
					Rect[] redoStackOfAddedCurtains = null;
					Rect[] redoStackOfRemovedCurtains = null;
					StrokeCollection[] redoStrokeCollectionStack = null;
					Stroke[] redoStrokeStack = null;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 2210:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 5:
								goto IL_001e;
							case 6:
								goto IL_002d;
							case 7:
								goto IL_0035;
							case 8:
								goto IL_003d;
							case 9:
								goto IL_004d;
							case 10:
								goto IL_0074;
							case 11:
								goto IL_0089;
							case 12:
								goto IL_00a5;
							case 13:
								goto IL_00bb;
							case 14:
								goto IL_00dc;
							case 15:
								goto IL_00fc;
							case 17:
								goto IL_011d;
							case 16:
							case 18:
								goto IL_0137;
							case 19:
								goto IL_0148;
							case 20:
								goto IL_0152;
							case 21:
								goto IL_016b;
							case 22:
								goto IL_0181;
							case 23:
								goto IL_0190;
							case 24:
								goto IL_01a1;
							case 25:
								goto IL_01ab;
							case 26:
								goto IL_01c7;
							case 27:
								goto IL_01dd;
							case 28:
								goto IL_01f5;
							case 29:
								goto IL_020c;
							case 30:
								goto IL_0222;
							case 31:
								goto IL_0239;
							case 33:
								goto IL_0274;
							case 32:
							case 34:
								goto IL_0291;
							case 35:
								goto IL_02a2;
							case 36:
								goto IL_02ac;
							case 37:
								goto IL_02c5;
							case 38:
								goto IL_02db;
							case 39:
								goto IL_02ea;
							case 40:
								goto IL_02fb;
							case 41:
								goto IL_0305;
							case 42:
								goto IL_0321;
							case 43:
								goto IL_0337;
							case 44:
								goto IL_0360;
							case 45:
								goto IL_0383;
							case 46:
								goto IL_0392;
							case 47:
								goto IL_03c9;
							case 48:
								goto IL_03da;
							case 49:
								goto IL_03e4;
							case 50:
								goto IL_0400;
							case 51:
								goto IL_0416;
							case 52:
								goto IL_043f;
							case 53:
								goto IL_0468;
							case 54:
								goto IL_0477;
							case 55:
								goto IL_0496;
							case 56:
								goto IL_04a7;
							case 57:
								goto IL_04b1;
							case 58:
								goto IL_04cd;
							case 59:
								goto IL_04e3;
							case 60:
								goto IL_050c;
							case 61:
								goto IL_0530;
							case 62:
								goto IL_0543;
							case 63:
								goto IL_0565;
							case 64:
								goto IL_0590;
							case 65:
								goto IL_05ab;
							case 66:
								goto IL_05bc;
							case 67:
								goto IL_05c6;
							case 68:
								goto IL_0609;
							case 69:
								goto IL_0623;
							case 70:
								goto IL_064c;
							case 71:
								goto IL_0682;
							case 72:
								goto IL_06b8;
							case 73:
								goto IL_06ca;
							case 74:
								goto IL_06d3;
							case 75:
								goto IL_06de;
							case 77:
								goto IL_06ef;
							case 76:
							case 78:
								goto IL_06fe;
							case 79:
								goto IL_0707;
							case 80:
								goto IL_0712;
							case 81:
								goto IL_071d;
							case 83:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 4:
							case 82:
							case 84:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_071d:
						num2 = 81;
						imgGrayedUndoRight.Visibility = Visibility.Visible;
						goto end_IL_0000_3;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						if (FrameNo == 0L)
						{
							goto end_IL_0000_3;
						}
						goto IL_001e;
						IL_001e:
						num2 = 5;
						if (cnvLibrary.Visibility == Visibility.Visible)
						{
							goto IL_002d;
						}
						goto IL_0035;
						IL_002d:
						num2 = 6;
						PlaceLibraryImageToBack();
						goto IL_0035;
						IL_0035:
						num2 = 7;
						isUndoneProp = new isUndoneProp();
						goto IL_003d;
						IL_003d:
						num2 = 8;
						iRedo++;
						goto IL_004d;
						IL_004d:
						num2 = 9;
						redo = Redo;
						redo = (isUndoneProp[])Utils.CopyArray(redo, new isUndoneProp[iRedo + 1]);
						Redo = redo;
						goto IL_0074;
						IL_0074:
						num2 = 10;
						Redo[iRedo] = new isUndoneProp();
						goto IL_0089;
						IL_0089:
						num2 = 11;
						if (FrameNo == FrameNoOfBackgroundPaperAdded[iAddedBackgroundPapers])
						{
							goto IL_00a5;
						}
						goto IL_0152;
						IL_00a5:
						num2 = 12;
						Redo[iRedo].BackgroundPaperAdded = true;
						goto IL_00bb;
						IL_00bb:
						num2 = 13;
						num5 = cnvBackgroundPaper.Children.Count - 1;
						i = num5;
						goto IL_012e;
						IL_012e:
						if (i >= 0)
						{
							goto IL_00dc;
						}
						goto IL_0137;
						IL_00dc:
						num2 = 14;
						if (cnvBackgroundPaper.Children[i].Visibility == Visibility.Visible)
						{
							goto IL_00fc;
						}
						goto IL_011d;
						IL_00fc:
						num2 = 15;
						cnvBackgroundPaper.Children[i].Visibility = Visibility.Collapsed;
						goto IL_0137;
						IL_0137:
						num2 = 18;
						iAddedBackgroundPapers--;
						goto IL_0148;
						IL_0148:
						num2 = 19;
						isUndoneProp.BackgroundPaperAdded = true;
						goto IL_0152;
						IL_011d:
						num2 = 17;
						i += -1;
						goto IL_012e;
						IL_0152:
						num2 = 20;
						if (FrameNo == FrameNoOfBackgroundCollapsed[iCollapsedBackgrounds])
						{
							goto IL_016b;
						}
						goto IL_01ab;
						IL_016b:
						num2 = 21;
						Redo[iRedo].BackgroundCollapsed = true;
						goto IL_0181;
						IL_0181:
						num2 = 22;
						cnvBackgroundPaper.Visibility = Visibility.Visible;
						goto IL_0190;
						IL_0190:
						num2 = 23;
						iCollapsedBackgrounds--;
						goto IL_01a1;
						IL_01a1:
						num2 = 24;
						isUndoneProp.BackgroundCollapsed = true;
						goto IL_01ab;
						IL_01ab:
						num2 = 25;
						if (FrameNo == FrameNoOfLibraryItemAdded[iAddedLibraryItems])
						{
							goto IL_01c7;
						}
						goto IL_02ac;
						IL_01c7:
						num2 = 26;
						Redo[iRedo].LibraryItemAdded = true;
						goto IL_01dd;
						IL_01dd:
						num2 = 27;
						num6 = iLibBack;
						i = num6;
						goto IL_0285;
						IL_0285:
						if (i >= 1)
						{
							goto IL_01f5;
						}
						goto IL_0291;
						IL_01f5:
						num2 = 28;
						if (imgLibBack[i].Visibility == Visibility.Visible)
						{
							goto IL_020c;
						}
						goto IL_0274;
						IL_020c:
						num2 = 29;
						imgLibBack[i].Visibility = Visibility.Collapsed;
						goto IL_0222;
						IL_0222:
						num2 = 30;
						if (propImgLibBack[i].RemovedImageIndex != 0)
						{
							goto IL_0239;
						}
						goto IL_0291;
						IL_0239:
						num2 = 31;
						AnimationFadeIn(imgLibBack[propImgLibBack[i].RemovedImageIndex], 300);
						goto IL_0291;
						IL_0291:
						num2 = 34;
						iAddedLibraryItems--;
						goto IL_02a2;
						IL_02a2:
						num2 = 35;
						isUndoneProp.LibraryItemAdded = true;
						goto IL_02ac;
						IL_0274:
						num2 = 33;
						i += -1;
						goto IL_0285;
						IL_02ac:
						num2 = 36;
						if (FrameNo == FrameNoOfLibraryCollapsed[iCollapsedLibraries])
						{
							goto IL_02c5;
						}
						goto IL_0305;
						IL_02c5:
						num2 = 37;
						Redo[iRedo].LibraryCollapsed = true;
						goto IL_02db;
						IL_02db:
						num2 = 38;
						cnvLibraryBack.Visibility = Visibility.Visible;
						goto IL_02ea;
						IL_02ea:
						num2 = 39;
						iCollapsedLibraries--;
						goto IL_02fb;
						IL_02fb:
						num2 = 40;
						isUndoneProp.LibraryCollapsed = true;
						goto IL_0305;
						IL_0305:
						num2 = 41;
						if (FrameNo == FrameNoOfCurtainAdded[iAddedCurtains])
						{
							goto IL_0321;
						}
						goto IL_03e4;
						IL_0321:
						num2 = 42;
						Redo[iRedo].CurtainAdded = true;
						goto IL_0337;
						IL_0337:
						num2 = 43;
						redoStackOfAddedCurtains = RedoStackOfAddedCurtains;
						redoStackOfAddedCurtains = (Rect[])Utils.CopyArray(redoStackOfAddedCurtains, new Rect[RedoStackOfAddedCurtains.Length + 1]);
						RedoStackOfAddedCurtains = redoStackOfAddedCurtains;
						goto IL_0360;
						IL_0360:
						num2 = 44;
						RedoStackOfAddedCurtains[RedoStackOfAddedCurtains.Length - 1] = rectTransparent.Rect;
						goto IL_0383;
						IL_0383:
						num2 = 45;
						cnvCurtain.Visibility = Visibility.Collapsed;
						goto IL_0392;
						IL_0392:
						num2 = 46;
						rectTransparent.Rect = new Rect(0.0, 0.0, 0.0, 0.0);
						goto IL_03c9;
						IL_03c9:
						num2 = 47;
						iAddedCurtains--;
						goto IL_03da;
						IL_03da:
						num2 = 48;
						isUndoneProp.CurtainAdded = true;
						goto IL_03e4;
						IL_03e4:
						num2 = 49;
						if (FrameNo == FrameNoOfCurtainRemoved[iRemovedCurtains])
						{
							goto IL_0400;
						}
						goto IL_04b1;
						IL_0400:
						num2 = 50;
						Redo[iRedo].CurtainRemoved = true;
						goto IL_0416;
						IL_0416:
						num2 = 51;
						redoStackOfRemovedCurtains = RedoStackOfRemovedCurtains;
						redoStackOfRemovedCurtains = (Rect[])Utils.CopyArray(redoStackOfRemovedCurtains, new Rect[RedoStackOfRemovedCurtains.Length + 1]);
						RedoStackOfRemovedCurtains = redoStackOfRemovedCurtains;
						goto IL_043f;
						IL_043f:
						num2 = 52;
						RedoStackOfRemovedCurtains[RedoStackOfRemovedCurtains.Length - 1] = UndoStackOfRemovedCurtains[iRemovedCurtains];
						goto IL_0468;
						IL_0468:
						num2 = 53;
						cnvCurtain.Visibility = Visibility.Visible;
						goto IL_0477;
						IL_0477:
						num2 = 54;
						rectTransparent.Rect = UndoStackOfRemovedCurtains[iRemovedCurtains];
						goto IL_0496;
						IL_0496:
						num2 = 55;
						iRemovedCurtains--;
						goto IL_04a7;
						IL_04a7:
						num2 = 56;
						isUndoneProp.CurtainRemoved = true;
						goto IL_04b1;
						IL_04b1:
						num2 = 57;
						if (FrameNo == FrameNoOfErase[iErase])
						{
							goto IL_04cd;
						}
						goto IL_05c6;
						IL_04cd:
						num2 = 58;
						Redo[iRedo].StrokeCollection = true;
						goto IL_04e3;
						IL_04e3:
						num2 = 59;
						redoStrokeCollectionStack = RedoStrokeCollectionStack;
						redoStrokeCollectionStack = (StrokeCollection[])Utils.CopyArray(redoStrokeCollectionStack, new StrokeCollection[RedoStrokeCollectionStack.Length + 1]);
						RedoStrokeCollectionStack = redoStrokeCollectionStack;
						goto IL_050c;
						IL_050c:
						num2 = 60;
						RedoStrokeCollectionStack[RedoStrokeCollectionStack.Length - 1] = inkCanvas.Strokes.Clone();
						goto IL_0530;
						IL_0530:
						num2 = 61;
						inkCanvas.Strokes.Clear();
						goto IL_0543;
						IL_0543:
						num2 = 62;
						num7 = UndoStrokeCollectionStack[iErase].Count - 1;
						i = 0;
						goto IL_05a1;
						IL_05a1:
						if (i <= num7)
						{
							goto IL_0565;
						}
						goto IL_05ab;
						IL_05ab:
						num2 = 65;
						iErase--;
						goto IL_05bc;
						IL_05bc:
						num2 = 66;
						isUndoneProp.StrokeCollection = true;
						goto IL_05c6;
						IL_0565:
						num2 = 63;
						inkCanvas.Strokes.Add(UndoStrokeCollectionStack[iErase][i]);
						goto IL_0590;
						IL_0590:
						num2 = 64;
						i++;
						goto IL_05a1;
						IL_05c6:
						num2 = 67;
						if ((inkCanvas.Strokes.Count > 0) & !isUndoneProp.CurtainAdded & !isUndoneProp.CurtainRemoved & !isUndoneProp.LibraryItemAdded & !isUndoneProp.StrokeCollection)
						{
							goto IL_0609;
						}
						goto IL_06b8;
						IL_0609:
						num2 = 68;
						Redo[Redo.Length - 1].Stroke = true;
						goto IL_0623;
						IL_0623:
						num2 = 69;
						redoStrokeStack = RedoStrokeStack;
						redoStrokeStack = (Stroke[])Utils.CopyArray(redoStrokeStack, new Stroke[RedoStrokeStack.Length + 1]);
						RedoStrokeStack = redoStrokeStack;
						goto IL_064c;
						IL_064c:
						num2 = 70;
						RedoStrokeStack[RedoStrokeStack.Length - 1] = inkCanvas.Strokes[inkCanvas.Strokes.Count - 1];
						goto IL_0682;
						IL_0682:
						num2 = 71;
						inkCanvas.Strokes.Remove(inkCanvas.Strokes[inkCanvas.Strokes.Count - 1]);
						goto IL_06b8;
						IL_06b8:
						num2 = 72;
						FrameNo--;
						goto IL_06ca;
						IL_06ca:
						num2 = 73;
						SelectRedoImageOfFav();
						goto IL_06d3;
						IL_06d3:
						num2 = 74;
						if (!isLeftSubMenu)
						{
							goto IL_06de;
						}
						goto IL_06ef;
						IL_06de:
						num2 = 75;
						imgGrayedRedoRight.Visibility = Visibility.Collapsed;
						goto IL_06fe;
						IL_06ef:
						num2 = 77;
						imgGrayedRedoLeft.Visibility = Visibility.Collapsed;
						goto IL_06fe;
						IL_06fe:
						num2 = 78;
						SelectUndoImageOfFav();
						goto IL_0707;
						IL_0707:
						num2 = 79;
						if (FrameNo != 0L)
						{
							goto end_IL_0000_3;
						}
						goto IL_0712;
						IL_0712:
						num2 = 80;
						if (isLeftSubMenu)
						{
							break;
						}
						goto IL_071d;
						end_IL_0000_2:
						break;
					}
					num2 = 83;
					imgGrayedUndoLeft.Visibility = Visibility.Visible;
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 2210;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void RedoIt()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		int num5 = default(int);
		int num7 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								int num6;
					Rect[] redoStackOfRemovedCurtains = null;
					Rect[] redoStackOfAddedCurtains = null;
					StrokeCollection[] redoStrokeCollectionStack = null;
					Stroke[] redoStrokeStack = null;
					isUndoneProp[] redo = null;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1705:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 5:
								goto IL_001e;
							case 6:
								goto IL_002d;
							case 7:
								goto IL_0035;
							case 8:
								goto IL_004f;
							case 9:
								goto IL_005d;
							case 10:
								goto IL_006e;
							case 11:
								goto IL_008c;
							case 12:
								goto IL_00ab;
							case 13:
								goto IL_00cc;
							case 14:
								goto IL_00eb;
							case 16:
								goto IL_00fc;
							case 15:
							case 17:
								goto IL_0116;
							case 18:
								goto IL_0127;
							case 19:
								goto IL_0142;
							case 20:
								goto IL_0151;
							case 21:
								goto IL_0162;
							case 22:
								goto IL_0180;
							case 23:
								goto IL_0198;
							case 24:
								goto IL_01a4;
							case 27:
								goto IL_01bf;
							case 28:
								goto IL_01d8;
							case 29:
								goto IL_01ee;
							case 30:
								goto IL_0205;
							case 26:
							case 32:
								goto IL_0229;
							case 25:
							case 31:
							case 33:
								goto IL_0246;
							case 34:
								goto IL_0257;
							case 35:
								goto IL_0275;
							case 36:
								goto IL_0284;
							case 37:
								goto IL_02bb;
							case 38:
								goto IL_02cc;
							case 39:
								goto IL_02f7;
							case 40:
								goto IL_0312;
							case 41:
								goto IL_0321;
							case 42:
								goto IL_0344;
							case 43:
								goto IL_0355;
							case 44:
								goto IL_0380;
							case 45:
								goto IL_039e;
							case 46:
								goto IL_03b1;
							case 47:
								goto IL_03d7;
							case 48:
								goto IL_0406;
							case 49:
								goto IL_0421;
							case 50:
								goto IL_0432;
							case 51:
								goto IL_045d;
							case 52:
								goto IL_0478;
							case 53:
								goto IL_049c;
							case 54:
								goto IL_04c7;
							case 55:
								goto IL_04d8;
							case 56:
								goto IL_04ff;
							case 57:
								goto IL_0511;
							case 58:
								goto IL_051a;
							case 59:
								goto IL_0525;
							case 61:
								goto IL_0536;
							case 60:
							case 62:
								goto IL_0545;
							case 63:
								goto IL_054e;
							case 64:
								goto IL_0559;
							case 65:
								goto IL_0564;
							case 67:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 4:
							case 66:
							case 68:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_0564:
						num2 = 65;
						imgGrayedRedoRight.Visibility = Visibility.Visible;
						goto end_IL_0000_3;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						if (iRedo == 0)
						{
							goto end_IL_0000_3;
						}
						goto IL_001e;
						IL_001e:
						num2 = 5;
						if (cnvLibrary.Visibility == Visibility.Visible)
						{
							goto IL_002d;
						}
						goto IL_0035;
						IL_002d:
						num2 = 6;
						PlaceLibraryImageToBack();
						goto IL_0035;
						IL_0035:
						num2 = 7;
						if (Redo[Redo.Length - 1].BackgroundCollapsed)
						{
							goto IL_004f;
						}
						goto IL_006e;
						IL_004f:
						num2 = 8;
						cnvBackgroundPaper.Visibility = Visibility.Collapsed;
						goto IL_005d;
						IL_005d:
						num2 = 9;
						iCollapsedBackgrounds++;
						goto IL_006e;
						IL_006e:
						num2 = 10;
						if (Redo[Redo.Length - 1].BackgroundPaperAdded)
						{
							goto IL_008c;
						}
						goto IL_0127;
						IL_008c:
						num2 = 11;
						num5 = cnvBackgroundPaper.Children.Count - 1;
						i = 0;
						goto IL_010d;
						IL_010d:
						if (i <= num5)
						{
							goto IL_00ab;
						}
						goto IL_0116;
						IL_00ab:
						num2 = 12;
						if (cnvBackgroundPaper.Children[i].Visibility == Visibility.Collapsed)
						{
							goto IL_00cc;
						}
						goto IL_00fc;
						IL_00cc:
						num2 = 13;
						cnvBackgroundPaper.Children[i].Visibility = Visibility.Visible;
						goto IL_00eb;
						IL_00eb:
						num2 = 14;
						cnvBackgroundPaper.Visibility = Visibility.Visible;
						goto IL_0116;
						IL_0116:
						num2 = 17;
						iAddedBackgroundPapers++;
						goto IL_0127;
						IL_00fc:
						num2 = 16;
						i++;
						goto IL_010d;
						IL_0127:
						num2 = 18;
						if (Redo[Redo.Length - 1].LibraryCollapsed)
						{
							goto IL_0142;
						}
						goto IL_0162;
						IL_0142:
						num2 = 19;
						cnvLibraryBack.Visibility = Visibility.Collapsed;
						goto IL_0151;
						IL_0151:
						num2 = 20;
						iCollapsedLibraries++;
						goto IL_0162;
						IL_0162:
						num2 = 21;
						if (Redo[Redo.Length - 1].LibraryItemAdded)
						{
							goto IL_0180;
						}
						goto IL_0257;
						IL_0180:
						num2 = 22;
						num6 = iLibBack;
						i = num6;
						goto IL_023a;
						IL_023a:
						if (i >= 1)
						{
							goto IL_0198;
						}
						goto IL_0246;
						IL_0198:
						num2 = 23;
						if (i == 1)
						{
							goto IL_01a4;
						}
						goto IL_01bf;
						IL_01a4:
						num2 = 24;
						imgLibBack[i].Visibility = Visibility.Visible;
						goto IL_0246;
						IL_01bf:
						num2 = 27;
						if (imgLibBack[i - 1].Visibility == Visibility.Visible)
						{
							goto IL_01d8;
						}
						goto IL_0229;
						IL_01d8:
						num2 = 28;
						imgLibBack[i].Visibility = Visibility.Visible;
						goto IL_01ee;
						IL_01ee:
						num2 = 29;
						if (propImgLibBack[i].RemovedImageIndex != 0)
						{
							goto IL_0205;
						}
						goto IL_0246;
						IL_0205:
						num2 = 30;
						imgLibBack[propImgLibBack[i].RemovedImageIndex].Visibility = Visibility.Collapsed;
						goto IL_0246;
						IL_0246:
						num2 = 33;
						iAddedLibraryItems++;
						goto IL_0257;
						IL_0229:
						num2 = 32;
						i += -1;
						goto IL_023a;
						IL_0257:
						num2 = 34;
						if (Redo[Redo.Length - 1].CurtainRemoved)
						{
							goto IL_0275;
						}
						goto IL_02f7;
						IL_0275:
						num2 = 35;
						cnvCurtain.Visibility = Visibility.Collapsed;
						goto IL_0284;
						IL_0284:
						num2 = 36;
						rectTransparent.Rect = new Rect(0.0, 0.0, 0.0, 0.0);
						goto IL_02bb;
						IL_02bb:
						num2 = 37;
						iRemovedCurtains++;
						goto IL_02cc;
						IL_02cc:
						num2 = 38;
						redoStackOfRemovedCurtains = RedoStackOfRemovedCurtains;
						redoStackOfRemovedCurtains = (Rect[])Utils.CopyArray(redoStackOfRemovedCurtains, new Rect[RedoStackOfRemovedCurtains.Length - 2 + 1]);
						RedoStackOfRemovedCurtains = redoStackOfRemovedCurtains;
						goto IL_02f7;
						IL_02f7:
						num2 = 39;
						if (Redo[Redo.Length - 1].CurtainAdded)
						{
							goto IL_0312;
						}
						goto IL_0380;
						IL_0312:
						num2 = 40;
						cnvCurtain.Visibility = Visibility.Visible;
						goto IL_0321;
						IL_0321:
						num2 = 41;
						rectTransparent.Rect = RedoStackOfAddedCurtains[RedoStackOfAddedCurtains.Length - 1];
						goto IL_0344;
						IL_0344:
						num2 = 42;
						iAddedCurtains++;
						goto IL_0355;
						IL_0355:
						num2 = 43;
						redoStackOfAddedCurtains = RedoStackOfAddedCurtains;
						redoStackOfAddedCurtains = (Rect[])Utils.CopyArray(redoStackOfAddedCurtains, new Rect[RedoStackOfAddedCurtains.Length - 2 + 1]);
						RedoStackOfAddedCurtains = redoStackOfAddedCurtains;
						goto IL_0380;
						IL_0380:
						num2 = 44;
						if (Redo[Redo.Length - 1].StrokeCollection)
						{
							goto IL_039e;
						}
						goto IL_045d;
						IL_039e:
						num2 = 45;
						inkCanvas.Strokes.Clear();
						goto IL_03b1;
						IL_03b1:
						num2 = 46;
						num7 = RedoStrokeCollectionStack[RedoStrokeCollectionStack.Length - 1].Count - 1;
						i = 0;
						goto IL_0417;
						IL_0417:
						if (i <= num7)
						{
							goto IL_03d7;
						}
						goto IL_0421;
						IL_0421:
						num2 = 49;
						iErase++;
						goto IL_0432;
						IL_0432:
						num2 = 50;
						redoStrokeCollectionStack = RedoStrokeCollectionStack;
						redoStrokeCollectionStack = (StrokeCollection[])Utils.CopyArray(redoStrokeCollectionStack, new StrokeCollection[RedoStrokeCollectionStack.Length - 2 + 1]);
						RedoStrokeCollectionStack = redoStrokeCollectionStack;
						goto IL_045d;
						IL_03d7:
						num2 = 47;
						inkCanvas.Strokes.Add(RedoStrokeCollectionStack[RedoStrokeCollectionStack.Length - 1][i]);
						goto IL_0406;
						IL_0406:
						num2 = 48;
						i++;
						goto IL_0417;
						IL_045d:
						num2 = 51;
						if (Redo[Redo.Length - 1].Stroke)
						{
							goto IL_0478;
						}
						goto IL_04c7;
						IL_0478:
						num2 = 52;
						inkCanvas.Strokes.Add(RedoStrokeStack[RedoStrokeStack.Length - 1]);
						goto IL_049c;
						IL_049c:
						num2 = 53;
						redoStrokeStack = RedoStrokeStack;
						redoStrokeStack = (Stroke[])Utils.CopyArray(redoStrokeStack, new Stroke[RedoStrokeStack.Length - 2 + 1]);
						RedoStrokeStack = redoStrokeStack;
						goto IL_04c7;
						IL_04c7:
						num2 = 54;
						iRedo--;
						goto IL_04d8;
						IL_04d8:
						num2 = 55;
						redo = Redo;
						redo = (isUndoneProp[])Utils.CopyArray(redo, new isUndoneProp[iRedo + 1]);
						Redo = redo;
						goto IL_04ff;
						IL_04ff:
						num2 = 56;
						FrameNo++;
						goto IL_0511;
						IL_0511:
						num2 = 57;
						SelectUndoImageOfFav();
						goto IL_051a;
						IL_051a:
						num2 = 58;
						if (!isLeftSubMenu)
						{
							goto IL_0525;
						}
						goto IL_0536;
						IL_0525:
						num2 = 59;
						imgGrayedUndoRight.Visibility = Visibility.Collapsed;
						goto IL_0545;
						IL_0536:
						num2 = 61;
						imgGrayedUndoLeft.Visibility = Visibility.Collapsed;
						goto IL_0545;
						IL_0545:
						num2 = 62;
						SelectRedoImageOfFav();
						goto IL_054e;
						IL_054e:
						num2 = 63;
						if (iRedo != 0)
						{
							goto end_IL_0000_3;
						}
						goto IL_0559;
						IL_0559:
						num2 = 64;
						if (isLeftSubMenu)
						{
							break;
						}
						goto IL_0564;
						end_IL_0000_2:
						break;
					}
					num2 = 67;
					imgGrayedRedoLeft.Visibility = Visibility.Visible;
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1705;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ResetRedo()
	{
		iRedo = 0;
		Redo = new isUndoneProp[1];
		RedoStrokeStack = new Stroke[1];
		RedoStrokeCollectionStack = new StrokeCollection[1];
		if (IndexOfFavRedo > 0)
		{
			imgFav[IndexOfFavRedo].Source = imgFavGrayedRedo.Source;
		}
	}

	private void ResetUndo()
	{
		UndoStrokeCollectionStack = new StrokeCollection[1];
		FrameNoOfErase = new int[1];
		FrameNo = 0L;
		iErase = 0;
		WaitForEraserMouseUp = false;
	}

	private void SelectUndoImageOfFav()
	{
		if (IndexOfFavUndo > 0)
		{
			if (FrameNo == 0L)
			{
				imgFav[IndexOfFavUndo].Source = imgFavGrayedUndo.Source;
			}
			else
			{
				imgFav[IndexOfFavUndo].Source = imgFavUndo.Source;
			}
		}
	}

	private void SelectRedoImageOfFav()
	{
		if (IndexOfFavRedo > 0)
		{
			if (iRedo == 0)
			{
				imgFav[IndexOfFavRedo].Source = imgFavGrayedRedo.Source;
			}
			else
			{
				imgFav[IndexOfFavRedo].Source = imgFavRedo.Source;
			}
		}
	}

	private void ShowCurtainSelection()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 430:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_0024;
							case 5:
								goto IL_0037;
							case 6:
								goto IL_0046;
							case 8:
								goto IL_0050;
							case 9:
								goto IL_0061;
							case 10:
								goto IL_006c;
							case 11:
								goto IL_0075;
							case 12:
								goto IL_0082;
							case 7:
							case 13:
								goto IL_008b;
							case 14:
								goto IL_009a;
							case 15:
								goto IL_00a5;
							case 16:
								goto IL_00dc;
							case 17:
								goto IL_00fc;
							case 18:
								goto IL_0122;
							case 19:
								goto IL_0131;
							case 20:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 21:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_0131:
						num2 = 19;
						if (ShownSubMenu == null)
						{
							goto end_IL_0000_3;
						}
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						cnvCurtain.Width = base.Width;
						goto IL_0024;
						IL_0024:
						num2 = 4;
						cnvCurtain.Height = base.Height;
						goto IL_0037;
						IL_0037:
						num2 = 5;
						if (cnvCurtain.Visibility == Visibility.Visible)
						{
							goto IL_0046;
						}
						goto IL_0050;
						IL_0046:
						num2 = 6;
						RemoveCurtain();
						goto IL_008b;
						IL_0050:
						num2 = 8;
						FrameNo++;
						goto IL_0061;
						IL_0061:
						num2 = 9;
						if (iRedo != 0)
						{
							goto IL_006c;
						}
						goto IL_0075;
						IL_006c:
						num2 = 10;
						ResetRedo();
						goto IL_0075;
						IL_0075:
						num2 = 11;
						if (FrameNo == 1)
						{
							goto IL_0082;
						}
						goto IL_008b;
						IL_0082:
						num2 = 12;
						SelectUndoImageOfFav();
						goto IL_008b;
						IL_008b:
						num2 = 13;
						DrawStateBeforeAction = DrawState;
						goto IL_009a;
						IL_009a:
						num2 = 14;
						DrawState = DrawStateEnum.Curtain;
						goto IL_00a5;
						IL_00a5:
						num2 = 15;
						rectTransparent.Rect = new Rect(0.0, 0.0, 0.0, 0.0);
						goto IL_00dc;
						IL_00dc:
						num2 = 16;
						pathCurtain.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 0, 0, 0));
						goto IL_00fc;
						IL_00fc:
						num2 = 17;
						AnimationFadeIn(cnvCurtain, 200, 0.0, 0.2);
						goto IL_0122;
						IL_0122:
						num2 = 18;
						inkCanvas.EditingMode = InkCanvasEditingMode.None;
						goto IL_0131;
						end_IL_0000_2:
						break;
					}
					num2 = 20;
					CollapseSubMenus();
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 430;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void PlaceCurtain()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								DrawStateEnum drawStateBeforeAction;
					Rect[] undoStackOfAddedCurtains = null;
					int[] frameNoOfCurtainAdded = null;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 433:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_001a;
							case 5:
								goto IL_0045;
							case 6:
								goto IL_0066;
							case 8:
								goto IL_0078;
							case 10:
								goto IL_0088;
							case 12:
								goto IL_0099;
							case 7:
							case 9:
							case 11:
							case 13:
							case 14:
								goto IL_00a8;
							case 15:
								goto IL_00b7;
							case 16:
								goto IL_00c8;
							case 17:
								goto IL_00ef;
							case 18:
								goto IL_0106;
							case 19:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 20:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_00ef:
						num2 = 17;
						FrameNoOfCurtainAdded[iAddedCurtains] = (int)FrameNo;
						goto IL_0106;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						Mouse.Capture(null);
						goto IL_001a;
						IL_001a:
						num2 = 4;
						pathCurtain.Fill = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
						goto IL_0045;
						IL_0045:
						num2 = 5;
						AnimationFadeIn(cnvCurtain, 1, 0.2);
						goto IL_0066;
						IL_0066:
						num2 = 6;
						drawStateBeforeAction = DrawStateBeforeAction;
						if (drawStateBeforeAction == DrawStateEnum.Pen)
						{
							goto IL_0078;
						}
						if (drawStateBeforeAction == DrawStateEnum.Eraser)
						{
							goto IL_0088;
						}
						goto IL_0099;
						IL_0106:
						num2 = 18;
						undoStackOfAddedCurtains = UndoStackOfAddedCurtains;
						undoStackOfAddedCurtains = (Rect[])Utils.CopyArray(undoStackOfAddedCurtains, new Rect[iAddedCurtains + 1]);
						UndoStackOfAddedCurtains = undoStackOfAddedCurtains;
						break;
						IL_0099:
						num2 = 12;
						inkCanvas.EditingMode = InkCanvasEditingMode.None;
						goto IL_00a8;
						IL_0088:
						num2 = 10;
						inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
						goto IL_00a8;
						IL_0078:
						num2 = 8;
						inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
						goto IL_00a8;
						IL_00a8:
						num2 = 14;
						DrawState = DrawStateBeforeAction;
						goto IL_00b7;
						IL_00b7:
						num2 = 15;
						iAddedCurtains++;
						goto IL_00c8;
						IL_00c8:
						num2 = 16;
						frameNoOfCurtainAdded = FrameNoOfCurtainAdded;
						frameNoOfCurtainAdded = (int[])Utils.CopyArray(frameNoOfCurtainAdded, new int[iAddedCurtains + 1]);
						FrameNoOfCurtainAdded = frameNoOfCurtainAdded;
						goto IL_00ef;
						end_IL_0000_2:
						break;
					}
					num2 = 19;
					UndoStackOfAddedCurtains[iAddedCurtains] = rectTransparent.Rect;
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 433;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void RemoveCurtain()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								int[] frameNoOfCurtainRemoved = null;
					Rect[] undoStackOfRemovedCurtains = null;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 369:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_0022;
							case 5:
								goto IL_002c;
							case 6:
								goto IL_0034;
							case 7:
								goto IL_0040;
							case 8:
								goto IL_0048;
							case 9:
								goto IL_0058;
							case 10:
								goto IL_007d;
							case 11:
								goto IL_0094;
							case 12:
								goto IL_00bb;
							case 13:
								goto IL_00da;
							case 14:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 15:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_00da:
						num2 = 13;
						cnvCurtain.Visibility = Visibility.Collapsed;
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						FrameNo++;
						goto IL_0022;
						IL_0022:
						num2 = 4;
						if (iRedo != 0)
						{
							goto IL_002c;
						}
						goto IL_0034;
						IL_002c:
						num2 = 5;
						ResetRedo();
						goto IL_0034;
						IL_0034:
						num2 = 6;
						if (FrameNo == 1)
						{
							goto IL_0040;
						}
						goto IL_0048;
						IL_0040:
						num2 = 7;
						SelectUndoImageOfFav();
						goto IL_0048;
						IL_0048:
						num2 = 8;
						iRemovedCurtains++;
						goto IL_0058;
						IL_0058:
						num2 = 9;
						frameNoOfCurtainRemoved = FrameNoOfCurtainRemoved;
						frameNoOfCurtainRemoved = (int[])Utils.CopyArray(frameNoOfCurtainRemoved, new int[iRemovedCurtains + 1]);
						FrameNoOfCurtainRemoved = frameNoOfCurtainRemoved;
						goto IL_007d;
						IL_007d:
						num2 = 10;
						FrameNoOfCurtainRemoved[iRemovedCurtains] = (int)FrameNo;
						goto IL_0094;
						IL_0094:
						num2 = 11;
						undoStackOfRemovedCurtains = UndoStackOfRemovedCurtains;
						undoStackOfRemovedCurtains = (Rect[])Utils.CopyArray(undoStackOfRemovedCurtains, new Rect[iRemovedCurtains + 1]);
						UndoStackOfRemovedCurtains = undoStackOfRemovedCurtains;
						goto IL_00bb;
						IL_00bb:
						num2 = 12;
						UndoStackOfRemovedCurtains[iRemovedCurtains] = rectTransparent.Rect;
						goto IL_00da;
						end_IL_0000_2:
						break;
					}
					num2 = 14;
					rectTransparent.Rect = new Rect(0.0, 0.0, 0.0, 0.0);
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 369;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ActivateLibrary()
	{
		if (ShownSubMenu != null)
		{
			CollapseSubMenus();
		}
		if (cnvLibrary.Visibility == Visibility.Visible)
		{
			PlaceLibraryImageToBack();
		}
		isGestureEnabled = false;
		Mouse.SetCursor(Cursors.Wait);
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			DefaultExt = ".png",
			Filter = "Resim Dosyaları|*.jpeg;*.jpg;*.png;*.gif;*.bmp;*.tif;",
			Multiselect = false
		};
		if (Operators.CompareString(LastBrowsedPath, null, TextCompare: false) == 0)
		{
			openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Görseller";
		}
		else if (!LastBrowsedPath.Contains(AppDomain.CurrentDomain.BaseDirectory + "Görseller"))
		{
			openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Görseller";
		}
		Mouse.SetCursor(Cursors.Arrow);
		bool? flag = openFileDialog.ShowDialog();
		flag = flag;
		checked
		{
			if (flag == true)
			{
				LastBrowsedPath = openFileDialog.FileName;
				string fileName = openFileDialog.FileName;
				try
				{
					BitmapImage bitmapImage = new BitmapImage(new Uri(fileName));
					if (bitmapImage.Width >= bitmapImage.Height)
					{
						imgLibrary.Width = 200.0;
						imgLibrary.Height = imgLibrary.Width * (bitmapImage.Height / bitmapImage.Width);
					}
					else
					{
						imgLibrary.Height = 200.0;
						imgLibrary.Width = imgLibrary.Height * (bitmapImage.Width / bitmapImage.Height);
					}
					imgLibrary.Tag = null;
					ActiveScaleRatio = 0.0;
					LastScaleRatio = 1.0;
					transScaleOfLibrary = new ScaleTransform(LastScaleRatio, LastScaleRatio);
					imgLibrary.RenderTransform = transScaleOfLibrary;
					rectLibrary.Height = imgLibrary.Height * LastScaleRatio + 50.0;
					rectLibrary.Width = imgLibrary.Width * LastScaleRatio + 50.0;
					cnvLibrary.Height = imgLibrary.Height * LastScaleRatio + 100.0;
					cnvLibrary.Width = imgLibrary.Width * LastScaleRatio + 100.0;
					imgResize.Margin = new Thickness(cnvLibrary.Width - imgResize.Width, cnvLibrary.Height - imgResize.Height, 0.0, 0.0);
					imgLibrary.Source = bitmapImage;
					int num = (int)Math.Round((base.Width - cnvLibrary.Width) / 2.0);
					int num2 = (int)Math.Round(base.Top + (base.Height - cnvLibrary.Height) / 2.0);
					cnvLibrary.Margin = new Thickness(num, num2, 0.0, 0.0);
					rectLibraryBorder.Width = cnvLibrary.Width;
					rectLibraryBorder.Height = cnvLibrary.Height;
					rectLibraryImage.Width = imgLibrary.Width;
					rectLibraryImage.Height = imgLibrary.Height;
					AnimationFadeIn(cnvLibrary, 300);
					ActiveRotationAngle = 0.0;
					LastRotationAngle = ActiveRotationAngle;
					transRotateOfLibrary = new RotateTransform(0.0, 0.0, 0.0)
					{
						Angle = ActiveRotationAngle
					};
					cnvLibrary.RenderTransform = transRotateOfLibrary;
					return;
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					isGestureEnabled = true;
					ProjectData.ClearProjectError();
					return;
				}
			}
			isGestureEnabled = MySettingsProperty.Settings.isGestureEnabled;
		}
	}

	private void PlaceLibraryImageToBack()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double left = default(double);
		double top = default(double);
		RotateTransform renderTransform = default(RotateTransform);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								int[] frameNoOfLibraryItemAdded = null;
					int num5;
					Image[] reference = null;
					Image[] reference2 = null;
					LibraryItemProp[] reference3 = null;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1489:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_0024;
							case 5:
								goto IL_0037;
							case 6:
								goto IL_005c;
							case 7:
								goto IL_006e;
							case 8:
								goto IL_007e;
							case 9:
								goto IL_0090;
							case 10:
								goto IL_009f;
							case 11:
								goto IL_00a9;
							case 13:
								goto IL_00bd;
							case 15:
								goto IL_00ed;
							case 16:
								goto IL_0111;
							case 17:
								goto IL_0132;
							case 18:
								goto IL_015b;
							case 19:
								goto IL_016c;
							case 20:
							case 22:
								goto IL_0193;
							case 12:
							case 14:
							case 21:
							case 23:
								goto IL_01b0;
							case 24:
								goto IL_01c1;
							case 25:
								goto IL_01e8;
							case 26:
								goto IL_021c;
							case 27:
								goto IL_0232;
							case 28:
								goto IL_0253;
							case 29:
								goto IL_0280;
							case 30:
								goto IL_02ae;
							case 31:
								goto IL_02dd;
							case 32:
								goto IL_0304;
							case 33:
								goto IL_032b;
							case 34:
								goto IL_0341;
							case 35:
								goto IL_0398;
							case 36:
								goto IL_03af;
							case 37:
								goto IL_03d6;
							case 38:
								goto IL_040f;
							case 39:
								goto IL_0426;
							case 41:
								goto IL_043e;
							case 40:
							case 42:
								goto IL_0469;
							case 43:
								goto IL_047b;
							case 44:
								goto IL_0486;
							case 45:
								goto IL_048f;
							case 46:
								goto IL_049c;
							case 47:
								goto IL_04a5;
							case 48:
								goto IL_04b6;
							case 49:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 50:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_04b6:
						num2 = 48;
						frameNoOfLibraryItemAdded = FrameNoOfLibraryItemAdded;
						frameNoOfLibraryItemAdded = (int[])Utils.CopyArray(frameNoOfLibraryItemAdded, new int[iAddedLibraryItems + 1]);
						FrameNoOfLibraryItemAdded = frameNoOfLibraryItemAdded;
						break;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						cnvLibraryBack.Width = base.Width;
						goto IL_0024;
						IL_0024:
						num2 = 4;
						cnvLibraryBack.Height = base.Height;
						goto IL_0037;
						IL_0037:
						num2 = 5;
						AnimationFadeOut(cnvLibrary, 300);
						goto IL_005c;
						IL_005c:
						num2 = 6;
						isGestureEnabled = MySettingsProperty.Settings.isGestureEnabled;
						goto IL_006e;
						IL_006e:
						num2 = 7;
						if (cnvLibraryBack.Visibility == Visibility.Collapsed)
						{
							goto IL_007e;
						}
						goto IL_00bd;
						IL_007e:
						num2 = 8;
						cnvLibraryBack.Children.Clear();
						goto IL_0090;
						IL_0090:
						num2 = 9;
						imgLibBack = new Image[1];
						goto IL_009f;
						IL_009f:
						num2 = 10;
						iLibBack = 0;
						goto IL_00a9;
						IL_00a9:
						num2 = 11;
						cnvLibraryBack.Visibility = Visibility.Visible;
						goto IL_01b0;
						IL_00bd:
						num2 = 13;
						if (cnvLibraryBack.Children[cnvLibraryBack.Children.Count - 1].Visibility != Visibility.Collapsed)
						{
							goto IL_00ed;
						}
						goto IL_01b0;
						IL_00ed:
						num2 = 15;
						num5 = cnvLibraryBack.Children.Count - 1;
						i = num5;
						goto IL_01a4;
						IL_01a4:
						if (i >= 0)
						{
							goto IL_0111;
						}
						goto IL_01b0;
						IL_0111:
						num2 = 16;
						if (cnvLibraryBack.Children[i].Visibility == Visibility.Collapsed)
						{
							goto IL_0132;
						}
						goto IL_01b0;
						IL_0132:
						num2 = 17;
						cnvLibraryBack.Children.Remove(cnvLibraryBack.Children[i]);
						goto IL_015b;
						IL_015b:
						num2 = 18;
						iLibBack--;
						goto IL_016c;
						IL_016c:
						num2 = 19;
						reference = imgLibBack;
						reference = (Image[])Utils.CopyArray(reference, new Image[iLibBack + 1]);
						imgLibBack = reference;
						goto IL_0193;
						IL_0193:
						num2 = 22;
						i += -1;
						goto IL_01a4;
						IL_01b0:
						num2 = 23;
						iLibBack++;
						goto IL_01c1;
						IL_01c1:
						num2 = 24;
						reference2 = imgLibBack;
						reference2 = (Image[])Utils.CopyArray(reference2, new Image[iLibBack + 1]);
						imgLibBack = reference2;
						goto IL_01e8;
						IL_01e8:
						num2 = 25;
						imgLibBack[iLibBack] = new Image
						{
							Source = imgLibrary.Source,
							VerticalAlignment = VerticalAlignment.Top,
							HorizontalAlignment = HorizontalAlignment.Left
						};
						goto IL_021c;
						IL_021c:
						num2 = 26;
						RenderOptions.SetBitmapScalingMode(imgLibBack[iLibBack], BitmapScalingMode.HighQuality);
						goto IL_0232;
						IL_0232:
						num2 = 27;
						cnvLibraryBack.Children.Add(imgLibBack[iLibBack]);
						goto IL_0253;
						IL_0253:
						num2 = 28;
						left = cnvLibrary.Margin.Left + imgLibrary.Margin.Left;
						goto IL_0280;
						IL_0280:
						num2 = 29;
						top = cnvLibrary.Margin.Top + imgLibrary.Margin.Top;
						goto IL_02ae;
						IL_02ae:
						num2 = 30;
						imgLibBack[iLibBack].Margin = new Thickness(left, top, 0.0, 0.0);
						goto IL_02dd;
						IL_02dd:
						num2 = 31;
						imgLibBack[iLibBack].Width = imgLibrary.Width * LastScaleRatio;
						goto IL_0304;
						IL_0304:
						num2 = 32;
						imgLibBack[iLibBack].Height = imgLibrary.Height * LastScaleRatio;
						goto IL_032b;
						IL_032b:
						num2 = 33;
						imgLibBack[iLibBack].Stretch = Stretch.Uniform;
						goto IL_0341;
						IL_0341:
						num2 = 34;
						renderTransform = new RotateTransform(0.0, imgLibBack[iLibBack].Width / 2.0, imgLibBack[iLibBack].Height / 2.0)
						{
							Angle = ActiveRotationAngle
						};
						goto IL_0398;
						IL_0398:
						num2 = 35;
						imgLibBack[iLibBack].RenderTransform = renderTransform;
						goto IL_03af;
						IL_03af:
						num2 = 36;
						reference3 = propImgLibBack;
						reference3 = (LibraryItemProp[])Utils.CopyArray(reference3, new LibraryItemProp[iLibBack + 1]);
						propImgLibBack = reference3;
						goto IL_03d6;
						IL_03d6:
						num2 = 37;
						propImgLibBack[iLibBack] = new LibraryItemProp
						{
							Index = iLibBack,
							LastScaleRatio = LastScaleRatio,
							ActiveRotationAngle = ActiveRotationAngle
						};
						goto IL_040f;
						IL_040f:
						num2 = 38;
						if (Operators.ConditionalCompareObjectEqual(imgLibrary.Tag, null, TextCompare: false))
						{
							goto IL_0426;
						}
						goto IL_043e;
						IL_0426:
						num2 = 39;
						propImgLibBack[iLibBack].RemovedImageIndex = 0;
						goto IL_0469;
						IL_043e:
						num2 = 41;
						propImgLibBack[iLibBack].RemovedImageIndex = (int)Math.Round(Conversions.ToDouble(imgLibrary.Tag));
						goto IL_0469;
						IL_0469:
						num2 = 42;
						FrameNo++;
						goto IL_047b;
						IL_047b:
						num2 = 43;
						if (iRedo != 0)
						{
							goto IL_0486;
						}
						goto IL_048f;
						IL_0486:
						num2 = 44;
						ResetRedo();
						goto IL_048f;
						IL_048f:
						num2 = 45;
						if (FrameNo == 1)
						{
							goto IL_049c;
						}
						goto IL_04a5;
						IL_049c:
						num2 = 46;
						SelectUndoImageOfFav();
						goto IL_04a5;
						IL_04a5:
						num2 = 47;
						iAddedLibraryItems++;
						goto IL_04b6;
						end_IL_0000_2:
						break;
					}
					num2 = 49;
					FrameNoOfLibraryItemAdded[iAddedLibraryItems] = (int)FrameNo;
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1489;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void ReselectLibrary(Point p)
	{
		HitTestResult hitTestResult = VisualTreeHelper.HitTest(cnvLibraryBack, p);
		if (hitTestResult == null)
		{
			return;
		}
		if (cnvLibrary.Visibility == Visibility.Visible)
		{
			PlaceLibraryImageToBack();
		}
		Image image = (Image)hitTestResult.VisualHit;
		if (image.Visibility == Visibility.Collapsed)
		{
			return;
		}
		imgLibrary.Source = image.Source;
		transScaleOfLibrary = new ScaleTransform(0.0, 0.0);
		imgLibrary.RenderTransform = transScaleOfLibrary;
		transRotateOfLibrary = new RotateTransform(0.0, 0.0, 0.0);
		imgLibrary.RenderTransform = transRotateOfLibrary;
		imgLibrary.Width = image.Width;
		imgLibrary.Height = image.Height;
		image.Tag = "Marked";
		int num = iLibBack;
		checked
		{
			int num2 = default(int);
			for (i = 1; i <= num; i++)
			{
				if (Operators.ConditionalCompareObjectEqual(imgLibBack[i].Tag, "Marked", TextCompare: false))
				{
					image.Tag = "";
					num2 = i;
					imgLibrary.Tag = num2;
					break;
				}
			}
			ActiveScaleRatio = propImgLibBack[num2].LastScaleRatio;
			LastScaleRatio = 1.0;
			rectLibrary.Height = imgLibrary.Height + 50.0;
			rectLibrary.Width = imgLibrary.Width + 50.0;
			cnvLibrary.Height = imgLibrary.Height + 100.0;
			cnvLibrary.Width = imgLibrary.Width + 100.0;
			imgResize.Margin = new Thickness(cnvLibrary.Width - imgResize.Width, cnvLibrary.Height - imgResize.Height, 0.0, 0.0);
			int num3 = (int)Math.Round(image.Margin.Left - 50.0);
			int num4 = (int)Math.Round(image.Margin.Top - 50.0);
			cnvLibrary.Margin = new Thickness(num3, num4, 0.0, 0.0);
			rectLibraryBorder.Width = cnvLibrary.Width;
			rectLibraryBorder.Height = cnvLibrary.Height;
			rectLibraryImage.Width = imgLibrary.Width;
			rectLibraryImage.Height = imgLibrary.Height;
			ActiveRotationAngle = propImgLibBack[num2].ActiveRotationAngle;
			LastRotationAngle = ActiveRotationAngle;
			transRotateOfLibrary = new RotateTransform(0.0, 0.0, 0.0)
			{
				Angle = LastRotationAngle
			};
			cnvLibrary.RenderTransform = transRotateOfLibrary;
			rectLibrary.Opacity = 0.0;
			imgResize.Opacity = 0.0;
			imgRotate.Opacity = 0.0;
			AnimationFadeIn(cnvLibrary, 1);
			AnimationFadeOut(imgLibBack[num2], 1);
			AnimationFadeIn(rectLibrary, 300);
			AnimationFadeIn(imgResize, 300);
			AnimationFadeIn(imgRotate, 300);
		}
	}

	private void rectLibrary_MouseDown(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point position = default(Point);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 167:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001a;
						case 5:
							goto IL_0028;
						case 6:
							goto IL_0032;
						case 7:
							goto IL_0046;
						case 8:
							goto IL_005a;
						case 9:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 10:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_005a:
					num2 = 8;
					if (ShownSubMenu == null)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					DragControlOfLibrary = true;
					goto IL_001a;
					IL_001a:
					num2 = 4;
					Mouse.Capture(rectLibrary);
					goto IL_0028;
					IL_0028:
					num2 = 5;
					position = e.GetPosition(this);
					goto IL_0032;
					IL_0032:
					num2 = 6;
					Drag.X = position.X;
					goto IL_0046;
					IL_0046:
					num2 = 7;
					Drag.Y = position.Y;
					goto IL_005a;
					end_IL_0000_2:
					break;
				}
				num2 = 9;
				CollapseSubMenus();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 167;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void rectLibrary_MouseMove(object sender, MouseEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point position = default(Point);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 284:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001e;
						case 5:
							goto IL_002c;
						case 6:
							goto IL_0036;
						case 7:
							goto IL_00aa;
						case 8:
							goto IL_00be;
						case 10:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 9:
						case 11:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00be:
					num2 = 8;
					Drag.Y = position.Y;
					goto end_IL_0000_3;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (!DragControlOfLibrary)
					{
						goto end_IL_0000_3;
					}
					goto IL_001e;
					IL_001e:
					num2 = 4;
					if (e.LeftButton != MouseButtonState.Pressed)
					{
						break;
					}
					goto IL_002c;
					IL_002c:
					num2 = 5;
					position = e.GetPosition(this);
					goto IL_0036;
					IL_0036:
					num2 = 6;
					cnvLibrary.Margin = new Thickness(cnvLibrary.Margin.Left + position.X - Drag.X, cnvLibrary.Margin.Top + position.Y - Drag.Y, 0.0, 0.0);
					goto IL_00aa;
					IL_00aa:
					num2 = 7;
					Drag.X = position.X;
					goto IL_00be;
					end_IL_0000_2:
					break;
				}
				num2 = 10;
				DragControlOfLibrary = false;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 284;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void rectLibrary_MouseUp(object sender, MouseButtonEventArgs e)
	{
		Mouse.Capture(null);
		DragControlOfLibrary = false;
	}

	private void imgResize_MouseDown(object sender, MouseButtonEventArgs e)
	{
		Mouse.Capture(imgResize);
		DragControlOfScale = true;
		Point position = e.GetPosition(this);
		Drag.X = position.X;
		Drag.Y = position.Y;
		if (ShownSubMenu != null)
		{
			CollapseSubMenus();
		}
	}

	private void imgResize_MouseMove(object sender, MouseEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		double num5 = default(double);
		double num6 = default(double);
		Point position = default(Point);
		double num7 = default(double);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 563:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001e;
						case 5:
							goto IL_002c;
						case 6:
							goto IL_0036;
						case 7:
							goto IL_004d;
						case 8:
							goto IL_0063;
						case 9:
							goto IL_0079;
						case 10:
							goto IL_008f;
						case 11:
							goto IL_00a3;
						case 12:
							goto IL_00b5;
						case 13:
							goto IL_00cf;
						case 14:
							goto IL_00e3;
						case 15:
							goto IL_00fa;
						case 16:
							goto IL_0111;
						case 17:
							goto IL_012b;
						case 18:
							goto IL_0145;
						case 19:
							goto IL_015f;
						case 20:
							goto IL_0179;
						case 22:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 21:
						case 23:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0179:
					num2 = 20;
					imgResize.Margin = new Thickness(num5 + 50.0, num6 + 50.0, 0.0, 0.0);
					goto end_IL_0000_3;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (!DragControlOfScale)
					{
						goto end_IL_0000_3;
					}
					goto IL_001e;
					IL_001e:
					num2 = 4;
					if (e.LeftButton != MouseButtonState.Pressed)
					{
						break;
					}
					goto IL_002c;
					IL_002c:
					num2 = 5;
					position = e.GetPosition(this);
					goto IL_0036;
					IL_0036:
					num2 = 6;
					num7 = position.X - Drag.X;
					goto IL_004d;
					IL_004d:
					num2 = 7;
					_ = position.Y;
					_ = Drag.Y;
					goto IL_0063;
					IL_0063:
					num2 = 8;
					ActiveScaleRatio = num7 / rectLibraryImage.Width;
					goto IL_0079;
					IL_0079:
					num2 = 9;
					ActiveScaleRatio += LastScaleRatio;
					goto IL_008f;
					IL_008f:
					num2 = 10;
					if (ActiveScaleRatio < 0.1)
					{
						goto IL_00a3;
					}
					goto IL_00b5;
					IL_00a3:
					num2 = 11;
					ActiveScaleRatio = 0.1;
					goto IL_00b5;
					IL_00b5:
					num2 = 12;
					transScaleOfLibrary = new ScaleTransform(ActiveScaleRatio, ActiveScaleRatio);
					goto IL_00cf;
					IL_00cf:
					num2 = 13;
					imgLibrary.RenderTransform = transScaleOfLibrary;
					goto IL_00e3;
					IL_00e3:
					num2 = 14;
					num5 = rectLibraryImage.Width * ActiveScaleRatio;
					goto IL_00fa;
					IL_00fa:
					num2 = 15;
					num6 = rectLibraryImage.Height * ActiveScaleRatio;
					goto IL_0111;
					IL_0111:
					num2 = 16;
					cnvLibrary.Width = num5 + 100.0;
					goto IL_012b;
					IL_012b:
					num2 = 17;
					cnvLibrary.Height = num6 + 100.0;
					goto IL_0145;
					IL_0145:
					num2 = 18;
					rectLibrary.Width = num5 + 50.0;
					goto IL_015f;
					IL_015f:
					num2 = 19;
					rectLibrary.Height = num6 + 50.0;
					goto IL_0179;
					end_IL_0000_2:
					break;
				}
				num2 = 22;
				DragControlOfScale = false;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 563;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgResize_MouseUp(object sender, MouseButtonEventArgs e)
	{
		Mouse.Capture(null);
		DragControlOfScale = false;
		LastScaleRatio = ActiveScaleRatio;
	}

	private void imgRotate_MouseDown(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point point = default(Point);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 600:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001f;
						case 5:
							goto IL_0028;
						case 6:
							goto IL_005f;
						case 7:
							goto IL_0096;
						case 8:
							goto IL_00de;
						case 9:
							goto IL_0126;
						case 10:
							goto IL_0165;
						case 11:
							goto IL_017c;
						case 12:
							goto IL_0195;
						case 13:
							goto IL_01a9;
						case 14:
							goto IL_01c2;
						case 15:
							goto IL_01eb;
						case 16:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 17:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_01eb:
					num2 = 15;
					if (ShownSubMenu == null)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					Mouse.Capture(imgRotate);
					goto IL_001f;
					IL_001f:
					num2 = 4;
					DragControlOfRotation = true;
					goto IL_0028;
					IL_0028:
					num2 = 5;
					Drag.X = cnvLibrary.Margin.Left + cnvLibrary.Width / 2.0;
					goto IL_005f;
					IL_005f:
					num2 = 6;
					Drag.Y = cnvLibrary.Margin.Top + cnvLibrary.Height / 2.0;
					goto IL_0096;
					IL_0096:
					num2 = 7;
					point.X = cnvLibrary.Margin.Left + imgRotate.Margin.Left + imgRotate.Width / 2.0;
					goto IL_00de;
					IL_00de:
					num2 = 8;
					point.Y = cnvLibrary.Margin.Top + imgRotate.Margin.Top + imgRotate.Height / 2.0;
					goto IL_0126;
					IL_0126:
					num2 = 9;
					LastRotationAngle = Math.Atan((point.Y - Drag.Y) / (point.X - Drag.X)) / (Math.PI / 180.0);
					goto IL_0165;
					IL_0165:
					num2 = 10;
					if (point.X < Drag.X)
					{
						goto IL_017c;
					}
					goto IL_0195;
					IL_017c:
					num2 = 11;
					LastRotationAngle = 180.0 + LastRotationAngle;
					goto IL_0195;
					IL_0195:
					num2 = 12;
					if (LastRotationAngle < 0.0)
					{
						goto IL_01a9;
					}
					goto IL_01c2;
					IL_01a9:
					num2 = 13;
					LastRotationAngle = 360.0 + LastRotationAngle;
					goto IL_01c2;
					IL_01c2:
					num2 = 14;
					transRotateOfLibrary = new RotateTransform(0.0, 0.0, 0.0);
					goto IL_01eb;
					end_IL_0000_2:
					break;
				}
				num2 = 16;
				CollapseSubMenus();
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 600;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgRotate_MouseMove(object sender, MouseEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point position = default(Point);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 366:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001e;
						case 5:
							goto IL_002c;
						case 6:
							goto IL_0036;
						case 7:
							goto IL_0074;
						case 8:
							goto IL_008a;
						case 9:
							goto IL_00a2;
						case 10:
							goto IL_00b6;
						case 11:
							goto IL_00cf;
						case 12:
							goto IL_00e5;
						case 13:
							goto IL_00f9;
						case 15:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 14:
						case 16:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_00f9:
					num2 = 13;
					cnvLibrary.RenderTransform = transRotateOfLibrary;
					goto end_IL_0000_3;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (!DragControlOfRotation)
					{
						goto end_IL_0000_3;
					}
					goto IL_001e;
					IL_001e:
					num2 = 4;
					if (e.LeftButton != MouseButtonState.Pressed)
					{
						break;
					}
					goto IL_002c;
					IL_002c:
					num2 = 5;
					position = e.GetPosition(this);
					goto IL_0036;
					IL_0036:
					num2 = 6;
					ActiveRotationAngle = Math.Atan((position.Y - Drag.Y) / (position.X - Drag.X)) / (Math.PI / 180.0);
					goto IL_0074;
					IL_0074:
					num2 = 7;
					if (position.X < Drag.X)
					{
						goto IL_008a;
					}
					goto IL_00a2;
					IL_008a:
					num2 = 8;
					ActiveRotationAngle = 180.0 + ActiveRotationAngle;
					goto IL_00a2;
					IL_00a2:
					num2 = 9;
					if (ActiveRotationAngle < 0.0)
					{
						goto IL_00b6;
					}
					goto IL_00cf;
					IL_00b6:
					num2 = 10;
					ActiveRotationAngle = 360.0 + ActiveRotationAngle;
					goto IL_00cf;
					IL_00cf:
					num2 = 11;
					ActiveRotationAngle -= LastRotationAngle;
					goto IL_00e5;
					IL_00e5:
					num2 = 12;
					transRotateOfLibrary.Angle = ActiveRotationAngle;
					goto IL_00f9;
					end_IL_0000_2:
					break;
				}
				num2 = 15;
				DragControlOfScale = false;
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 366;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void imgRotate_MouseUp(object sender, MouseButtonEventArgs e)
	{
		Mouse.Capture(null);
		DragControlOfRotation = false;
	}

	private void SetFavIcon(Image img, string txtTag)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								int num5;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 1163:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 5:
								goto IL_0051;
							case 6:
								goto IL_0063;
							case 8:
								goto IL_0083;
							case 9:
								goto IL_009c;
							case 11:
								goto IL_00b8;
							case 12:
								goto IL_00ca;
							case 13:
								goto IL_010b;
							case 14:
								goto IL_012c;
							case 15:
								goto IL_014d;
							case 16:
								goto IL_016e;
							case 17:
								goto IL_017a;
							case 19:
								goto IL_01d5;
							case 18:
							case 20:
								goto IL_0245;
							case 21:
								goto IL_0260;
							case 22:
								goto IL_0271;
							case 23:
								goto IL_0280;
							case 24:
								goto IL_02a1;
							case 25:
								goto IL_02b2;
							case 26:
								goto IL_02c1;
							case 27:
								goto IL_02e2;
							case 28:
								goto IL_02f8;
							case 29:
								goto IL_0325;
							case 30:
								goto IL_0348;
							case 31:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 4:
							case 7:
							case 10:
							case 32:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_0083:
						num2 = 8;
						i += -1;
						goto IL_0093;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						if ((iFav >= (int)Math.Round(base.Width * 12.0 / 1366.0)) | (iFav >= 20))
						{
							goto end_IL_0000_3;
						}
						goto IL_0051;
						IL_0051:
						num2 = 5;
						num5 = iFav;
						i = num5;
						goto IL_0093;
						IL_0093:
						if (i >= 1)
						{
							goto IL_0063;
						}
						goto IL_009c;
						IL_009c:
						num2 = 9;
						if ((img == null) | (Operators.CompareString(txtTag, "", TextCompare: false) == 0))
						{
							goto end_IL_0000_3;
						}
						goto IL_00b8;
						IL_00b8:
						num2 = 11;
						iFav++;
						goto IL_00ca;
						IL_00ca:
						num2 = 12;
						imgFav[iFav] = new Image
						{
							HorizontalAlignment = HorizontalAlignment.Left,
							VerticalAlignment = VerticalAlignment.Top,
							Width = 42.0,
							Height = 42.0
						};
						goto IL_010b;
						IL_010b:
						num2 = 13;
						imgFav[iFav].MouseLeftButtonUp += Fav_MouseLeftButtonUp;
						goto IL_012c;
						IL_012c:
						num2 = 14;
						imgFav[iFav].MouseRightButtonUp += Fav_MouseRightButtonUp;
						goto IL_014d;
						IL_014d:
						num2 = 15;
						gridMainMenu.Children.Add(imgFav[iFav]);
						goto IL_016e;
						IL_016e:
						num2 = 16;
						if (iFav == 1)
						{
							goto IL_017a;
						}
						goto IL_01d5;
						IL_017a:
						num2 = 17;
						imgFav[1].Margin = new Thickness(imgShape.Margin.Left, imgShape.Margin.Top + 42.0, 0.0, 0.0);
						goto IL_0245;
						IL_01d5:
						num2 = 19;
						imgFav[iFav].Margin = new Thickness(imgFav[iFav - 1].Margin.Left, imgFav[iFav - 1].Margin.Top + 42.0, 0.0, 0.0);
						goto IL_0245;
						IL_0245:
						num2 = 20;
						imgFav[iFav].Source = img.Source;
						goto IL_0260;
						IL_0260:
						num2 = 21;
						if (Operators.CompareString(txtTag, "Undo", TextCompare: false) == 0)
						{
							goto IL_0271;
						}
						goto IL_02a1;
						IL_0271:
						num2 = 22;
						IndexOfFavUndo = iFav;
						goto IL_0280;
						IL_0280:
						num2 = 23;
						imgFav[iFav].Source = SelectFavImg(txtTag).Source;
						goto IL_02a1;
						IL_02a1:
						num2 = 24;
						if (Operators.CompareString(txtTag, "Redo", TextCompare: false) == 0)
						{
							goto IL_02b2;
						}
						goto IL_02e2;
						IL_02b2:
						num2 = 25;
						IndexOfFavRedo = iFav;
						goto IL_02c1;
						IL_02c1:
						num2 = 26;
						imgFav[iFav].Source = SelectFavImg(txtTag).Source;
						goto IL_02e2;
						IL_02e2:
						num2 = 27;
						imgFav[iFav].Tag = txtTag;
						goto IL_02f8;
						IL_02f8:
						num2 = 28;
						AnimationFadeIn(imgFav[iFav], 500);
						goto IL_0325;
						IL_0325:
						num2 = 29;
						borderMainMenu.Height += 42.0;
						goto IL_0348;
						IL_0348:
						num2 = 30;
						imgSettings.Margin = new Thickness(imgSettings.Margin.Left, imgSettings.Margin.Top + 42.0, 0.0, 0.0);
						break;
						IL_0063:
						num2 = 6;
						if (Operators.ConditionalCompareObjectEqual(imgFav[i].Tag, txtTag, TextCompare: false))
						{
							goto end_IL_0000_3;
						}
						goto IL_0083;
						end_IL_0000_2:
						break;
					}
					num2 = 31;
					imgClose.Margin = new Thickness(imgClose.Margin.Left, imgClose.Margin.Top + 42.0, 0.0, 0.0);
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1163;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void Fav_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		byte b = default(byte);
		int num6 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
								int num5;
					switch (try0000_dispatch)
					{
					default:
						num2 = 1;
						if (IgnoreErrors)
						{
							goto IL_000a;
						}
						goto IL_0011;
					case 810:
						{
							num = num2;
							switch (num3)
							{
							case 1:
								break;
							default:
								goto end_IL_0000;
							}
							int num4 = unchecked(num + 1);
							num = 0;
							switch (num4)
							{
							case 1:
								break;
							case 2:
								goto IL_000a;
							case 3:
								goto IL_0011;
							case 4:
								goto IL_001b;
							case 5:
								goto IL_0023;
							case 6:
								goto IL_007e;
							case 7:
								goto IL_009a;
							case 8:
								goto IL_00c2;
							case 9:
								goto IL_00ef;
							case 10:
								goto IL_011c;
							case 11:
								goto IL_013a;
							case 12:
								goto IL_015f;
							case 13:
								goto IL_0169;
							case 14:
								goto IL_018e;
							case 15:
								goto IL_0198;
							case 16:
								goto IL_01ae;
							case 17:
								goto IL_01c4;
							case 18:
								goto IL_01da;
							case 19:
								goto IL_01ec;
							case 20:
								goto IL_020f;
							case 21:
								goto end_IL_0000_2;
							default:
								goto end_IL_0000;
							case 22:
								goto end_IL_0000_3;
							}
							goto default;
						}
						IL_011c:
						num2 = 10;
						i++;
						goto IL_012d;
						IL_000a:
						ProjectData.ClearProjectError();
						num3 = 1;
						goto IL_0011;
						IL_0011:
						num2 = 3;
						if (ShownSubMenu != null)
						{
							goto IL_001b;
						}
						goto IL_0023;
						IL_001b:
						num2 = 4;
						CollapseSubMenus();
						goto IL_0023;
						IL_0023:
						num2 = 5;
						b = Conversions.ToByte(Operators.DivideObject(Operators.SubtractObject(NewLateBinding.LateGet(NewLateBinding.LateGet(sender, null, "margin", new object[0], null, null, null), null, "top", new object[0], null, null, null), imgShape.Margin.Top), 42));
						goto IL_007e;
						IL_007e:
						num2 = 6;
						num5 = b;
						num6 = iFav - 1;
						i = num5;
						goto IL_012d;
						IL_012d:
						if (i <= num6)
						{
							goto IL_009a;
						}
						goto IL_013a;
						IL_013a:
						num2 = 11;
						if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null), "Undo", TextCompare: false))
						{
							goto IL_015f;
						}
						goto IL_0169;
						IL_015f:
						num2 = 12;
						IndexOfFavUndo = 0;
						goto IL_0169;
						IL_0169:
						num2 = 13;
						if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null), "Redo", TextCompare: false))
						{
							goto IL_018e;
						}
						goto IL_0198;
						IL_018e:
						num2 = 14;
						IndexOfFavRedo = 0;
						goto IL_0198;
						IL_0198:
						num2 = 15;
						imgFav[iFav].Visibility = Visibility.Collapsed;
						goto IL_01ae;
						IL_01ae:
						num2 = 16;
						imgFav[iFav].Source = null;
						goto IL_01c4;
						IL_01c4:
						num2 = 17;
						imgFav[iFav].Tag = null;
						goto IL_01da;
						IL_01da:
						num2 = 18;
						iFav--;
						goto IL_01ec;
						IL_01ec:
						num2 = 19;
						borderMainMenu.Height -= 42.0;
						goto IL_020f;
						IL_020f:
						num2 = 20;
						imgSettings.Margin = new Thickness(imgSettings.Margin.Left, imgSettings.Margin.Top - 42.0, 0.0, 0.0);
						break;
						IL_009a:
						num2 = 7;
						imgFav[i].Source = imgFav[i + 1].Source;
						goto IL_00c2;
						IL_00c2:
						num2 = 8;
						imgFav[i].Tag = RuntimeHelpers.GetObjectValue(imgFav[i + 1].Tag);
						goto IL_00ef;
						IL_00ef:
						num2 = 9;
						AnimationFadeIn(imgFav[i], 500);
						goto IL_011c;
						end_IL_0000_2:
						break;
					}
					num2 = 21;
					imgClose.Margin = new Thickness(imgClose.Margin.Left, imgClose.Margin.Top - 42.0, 0.0, 0.0);
					break;
								end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 810;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void Fav_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		Point pFav = default(Point);
		object left = default(object);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 5993:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001b;
						case 5:
							goto IL_0023;
						case 7:
							goto IL_003c;
						case 8:
							goto IL_004d;
						case 9:
							goto IL_005f;
						case 10:
							goto IL_0072;
						case 12:
							goto IL_0083;
						case 13:
							goto IL_0095;
						case 14:
							goto IL_00a8;
						case 15:
							goto IL_00bb;
						case 17:
							goto IL_00cc;
						case 18:
							goto IL_00de;
						case 19:
							goto IL_00f1;
						case 20:
							goto IL_0104;
						case 21:
							goto IL_0110;
						case 22:
							goto IL_0123;
						case 23:
							goto IL_0136;
						case 25:
							goto IL_0147;
						case 26:
							goto IL_0159;
						case 27:
							goto IL_016c;
						case 28:
							goto IL_017f;
						case 29:
							goto IL_018b;
						case 30:
							goto IL_019e;
						case 31:
							goto IL_01b1;
						case 33:
							goto IL_01c2;
						case 34:
							goto IL_01d4;
						case 35:
							goto IL_01e7;
						case 36:
							goto IL_01fa;
						case 37:
							goto IL_0206;
						case 38:
							goto IL_0219;
						case 39:
							goto IL_022c;
						case 41:
							goto IL_023d;
						case 42:
							goto IL_024f;
						case 43:
							goto IL_0262;
						case 44:
							goto IL_0275;
						case 45:
							goto IL_0281;
						case 46:
							goto IL_0294;
						case 47:
							goto IL_02a7;
						case 49:
							goto IL_02b8;
						case 50:
							goto IL_02ca;
						case 51:
							goto IL_02dd;
						case 52:
							goto IL_02f0;
						case 53:
							goto IL_02fc;
						case 54:
							goto IL_030f;
						case 55:
							goto IL_0322;
						case 57:
							goto IL_0333;
						case 58:
							goto IL_0345;
						case 59:
							goto IL_0358;
						case 60:
							goto IL_036b;
						case 61:
							goto IL_0377;
						case 62:
							goto IL_038a;
						case 63:
							goto IL_039d;
						case 65:
							goto IL_03ae;
						case 66:
							goto IL_03c0;
						case 67:
							goto IL_03d3;
						case 68:
							goto IL_03e6;
						case 69:
							goto IL_03f2;
						case 70:
							goto IL_0405;
						case 71:
							goto IL_0418;
						case 73:
							goto IL_0429;
						case 74:
							goto IL_043b;
						case 75:
							goto IL_044e;
						case 76:
							goto IL_0461;
						case 77:
							goto IL_046d;
						case 78:
							goto IL_0480;
						case 79:
							goto IL_0493;
						case 81:
							goto IL_04a4;
						case 82:
							goto IL_04b6;
						case 83:
							goto IL_04c9;
						case 84:
							goto IL_04dc;
						case 85:
							goto IL_04e8;
						case 86:
							goto IL_04fb;
						case 87:
							goto IL_050e;
						case 89:
							goto IL_051f;
						case 90:
							goto IL_0531;
						case 91:
							goto IL_0544;
						case 92:
							goto IL_0557;
						case 93:
							goto IL_0563;
						case 94:
							goto IL_0576;
						case 95:
							goto IL_0589;
						case 97:
							goto IL_059a;
						case 98:
							goto IL_05ac;
						case 99:
							goto IL_05bf;
						case 100:
							goto IL_05d2;
						case 101:
							goto IL_05de;
						case 102:
							goto IL_05f1;
						case 103:
							goto IL_0604;
						case 105:
							goto IL_0615;
						case 106:
							goto IL_0627;
						case 107:
							goto IL_063a;
						case 108:
							goto IL_064d;
						case 109:
							goto IL_0659;
						case 110:
							goto IL_066c;
						case 111:
							goto IL_067f;
						case 113:
							goto IL_0690;
						case 114:
							goto IL_06a2;
						case 115:
							goto IL_06b5;
						case 116:
							goto IL_06c8;
						case 117:
							goto IL_06d4;
						case 118:
							goto IL_06e7;
						case 119:
							goto IL_06fa;
						case 121:
							goto IL_070b;
						case 122:
							goto IL_071d;
						case 123:
							goto IL_0730;
						case 124:
							goto IL_0743;
						case 125:
							goto IL_074f;
						case 126:
							goto IL_0762;
						case 127:
							goto IL_0775;
						case 129:
							goto IL_0786;
						case 130:
							goto IL_079b;
						case 131:
							goto IL_07b1;
						case 132:
							goto IL_07c7;
						case 133:
							goto IL_07d6;
						case 134:
							goto IL_07ec;
						case 135:
							goto IL_0802;
						case 137:
							goto IL_0816;
						case 138:
							goto IL_082b;
						case 139:
							goto IL_0841;
						case 140:
							goto IL_0857;
						case 141:
							goto IL_0866;
						case 142:
							goto IL_087c;
						case 143:
							goto IL_0892;
						case 145:
							goto IL_08a6;
						case 146:
							goto IL_08bb;
						case 147:
							goto IL_08d1;
						case 148:
							goto IL_08e7;
						case 149:
							goto IL_08f6;
						case 150:
							goto IL_090c;
						case 151:
							goto IL_0922;
						case 153:
							goto IL_0936;
						case 154:
							goto IL_094b;
						case 155:
							goto IL_0961;
						case 156:
							goto IL_0977;
						case 157:
							goto IL_0986;
						case 158:
							goto IL_099c;
						case 159:
							goto IL_09b2;
						case 161:
							goto IL_09c6;
						case 162:
							goto IL_09db;
						case 163:
							goto IL_09f1;
						case 164:
							goto IL_0a07;
						case 165:
							goto IL_0a16;
						case 166:
							goto IL_0a2c;
						case 167:
							goto IL_0a42;
						case 169:
							goto IL_0a56;
						case 170:
							goto IL_0a6b;
						case 171:
							goto IL_0a81;
						case 172:
							goto IL_0a97;
						case 173:
							goto IL_0aa6;
						case 174:
							goto IL_0abc;
						case 175:
							goto IL_0ad2;
						case 177:
							goto IL_0ae6;
						case 178:
							goto IL_0afb;
						case 179:
							goto IL_0b11;
						case 180:
							goto IL_0b27;
						case 181:
							goto IL_0b36;
						case 182:
							goto IL_0b4c;
						case 183:
							goto IL_0b62;
						case 185:
							goto IL_0b76;
						case 186:
							goto IL_0b8b;
						case 187:
							goto IL_0ba1;
						case 188:
							goto IL_0bb7;
						case 190:
							goto IL_0bcb;
						case 191:
							goto IL_0be0;
						case 192:
							goto IL_0bf6;
						case 193:
							goto IL_0c0c;
						case 195:
							goto IL_0c20;
						case 196:
							goto IL_0c35;
						case 197:
							goto IL_0c4b;
						case 198:
							goto IL_0c61;
						case 200:
							goto IL_0c75;
						case 201:
							goto IL_0c8a;
						case 202:
							goto IL_0ca0;
						case 203:
							goto IL_0cb6;
						case 205:
							goto IL_0cca;
						case 206:
							goto IL_0cdf;
						case 207:
							goto IL_0cf5;
						case 208:
							goto IL_0d0b;
						case 210:
							goto IL_0d1f;
						case 211:
							goto IL_0d34;
						case 212:
							goto IL_0d4a;
						case 213:
							goto IL_0d60;
						case 215:
							goto IL_0d74;
						case 216:
							goto IL_0d89;
						case 217:
							goto IL_0d9f;
						case 218:
							goto IL_0db5;
						case 220:
							goto IL_0dc9;
						case 221:
							goto IL_0dde;
						case 222:
							goto IL_0df4;
						case 223:
							goto IL_0e0a;
						case 225:
							goto IL_0e1e;
						case 226:
							goto IL_0e33;
						case 227:
							goto IL_0e49;
						case 228:
							goto IL_0e5f;
						case 230:
							goto IL_0e73;
						case 231:
							goto IL_0e88;
						case 232:
							goto IL_0e9e;
						case 233:
							goto IL_0eb4;
						case 235:
							goto IL_0ec8;
						case 236:
							goto IL_0edd;
						case 237:
							goto IL_0ef3;
						case 238:
							goto IL_0f09;
						case 240:
							goto IL_0f1d;
						case 241:
							goto IL_0f32;
						case 242:
							goto IL_0f48;
						case 243:
							goto IL_0f5e;
						case 245:
							goto IL_0f72;
						case 246:
							goto IL_0f87;
						case 247:
							goto IL_0f9d;
						case 248:
							goto IL_0fb3;
						case 250:
							goto IL_0fc7;
						case 251:
							goto IL_0fdc;
						case 252:
							goto IL_0ff2;
						case 253:
							goto IL_1008;
						case 255:
							goto IL_101c;
						case 256:
							goto IL_1031;
						case 257:
							goto IL_1047;
						case 258:
							goto IL_105d;
						case 260:
							goto IL_1071;
						case 261:
							goto IL_1086;
						case 262:
							goto IL_109c;
						case 263:
							goto IL_10b2;
						case 265:
							goto IL_10c6;
						case 266:
							goto IL_10db;
						case 267:
							goto IL_10f1;
						case 268:
							goto IL_1107;
						case 270:
							goto IL_111b;
						case 271:
							goto IL_1130;
						case 272:
							goto IL_1146;
						case 273:
							goto IL_115c;
						case 275:
							goto IL_1170;
						case 276:
							goto IL_1185;
						case 277:
							goto IL_119b;
						case 278:
							goto IL_11b1;
						case 280:
							goto IL_11c5;
						case 281:
							goto IL_11da;
						case 282:
							goto IL_11f0;
						case 283:
							goto IL_1206;
						case 285:
							goto IL_121a;
						case 286:
							goto IL_122f;
						case 287:
							goto IL_1245;
						case 288:
							goto IL_125b;
						case 290:
							goto IL_126c;
						case 291:
							goto IL_1281;
						case 292:
							goto IL_1297;
						case 293:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 6:
						case 11:
						case 16:
						case 24:
						case 32:
						case 40:
						case 48:
						case 56:
						case 64:
						case 72:
						case 80:
						case 88:
						case 96:
						case 104:
						case 112:
						case 120:
						case 128:
						case 136:
						case 144:
						case 152:
						case 160:
						case 168:
						case 176:
						case 184:
						case 189:
						case 194:
						case 199:
						case 204:
						case 209:
						case 214:
						case 219:
						case 224:
						case 229:
						case 234:
						case 239:
						case 244:
						case 249:
						case 254:
						case 259:
						case 264:
						case 269:
						case 274:
						case 279:
						case 284:
						case 289:
						case 294:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_1297:
					num2 = 292;
					pFav.Y = 95.0;
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (ShownSubMenu != null)
					{
						goto IL_001b;
					}
					goto IL_0023;
					IL_001b:
					num2 = 4;
					CollapseSubMenus();
					goto IL_0023;
					IL_0023:
					num2 = 5;
					left = NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null);
					goto IL_003c;
					IL_003c:
					num2 = 7;
					if (Operators.ConditionalCompareObjectEqual(left, "Undo", TextCompare: false))
					{
						goto IL_004d;
					}
					goto IL_0083;
					IL_004d:
					num2 = 8;
					pFav.X = 45.0;
					goto IL_005f;
					IL_005f:
					num2 = 9;
					pFav.Y = 30.0;
					goto IL_0072;
					IL_0072:
					num2 = 10;
					SubMenuEraser_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0083:
					num2 = 12;
					if (Operators.ConditionalCompareObjectEqual(left, "Redo", TextCompare: false))
					{
						goto IL_0095;
					}
					goto IL_00cc;
					IL_0095:
					num2 = 13;
					pFav.X = 84.0;
					goto IL_00a8;
					IL_00a8:
					num2 = 14;
					pFav.Y = 30.0;
					goto IL_00bb;
					IL_00bb:
					num2 = 15;
					SubMenuEraser_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_00cc:
					num2 = 17;
					if (Operators.ConditionalCompareObjectEqual(left, "MarkerRed", TextCompare: false))
					{
						goto IL_00de;
					}
					goto IL_0147;
					IL_00de:
					num2 = 18;
					pFav.X = 40.0;
					goto IL_00f1;
					IL_00f1:
					num2 = 19;
					pFav.Y = 20.0;
					goto IL_0104;
					IL_0104:
					num2 = 20;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0110;
					IL_0110:
					num2 = 21;
					pFav.X = 35.0;
					goto IL_0123;
					IL_0123:
					num2 = 22;
					pFav.Y = 50.0;
					goto IL_0136;
					IL_0136:
					num2 = 23;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0147:
					num2 = 25;
					if (Operators.ConditionalCompareObjectEqual(left, "StylographRed", TextCompare: false))
					{
						goto IL_0159;
					}
					goto IL_01c2;
					IL_0159:
					num2 = 26;
					pFav.X = 85.0;
					goto IL_016c;
					IL_016c:
					num2 = 27;
					pFav.Y = 20.0;
					goto IL_017f;
					IL_017f:
					num2 = 28;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_018b;
					IL_018b:
					num2 = 29;
					pFav.X = 35.0;
					goto IL_019e;
					IL_019e:
					num2 = 30;
					pFav.Y = 50.0;
					goto IL_01b1;
					IL_01b1:
					num2 = 31;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_01c2:
					num2 = 33;
					if (Operators.ConditionalCompareObjectEqual(left, "HighlighterRed", TextCompare: false))
					{
						goto IL_01d4;
					}
					goto IL_023d;
					IL_01d4:
					num2 = 34;
					pFav.X = 130.0;
					goto IL_01e7;
					IL_01e7:
					num2 = 35;
					pFav.Y = 20.0;
					goto IL_01fa;
					IL_01fa:
					num2 = 36;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0206;
					IL_0206:
					num2 = 37;
					pFav.X = 35.0;
					goto IL_0219;
					IL_0219:
					num2 = 38;
					pFav.Y = 50.0;
					goto IL_022c;
					IL_022c:
					num2 = 39;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_023d:
					num2 = 41;
					if (Operators.ConditionalCompareObjectEqual(left, "MarkerBlue", TextCompare: false))
					{
						goto IL_024f;
					}
					goto IL_02b8;
					IL_024f:
					num2 = 42;
					pFav.X = 40.0;
					goto IL_0262;
					IL_0262:
					num2 = 43;
					pFav.Y = 20.0;
					goto IL_0275;
					IL_0275:
					num2 = 44;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0281;
					IL_0281:
					num2 = 45;
					pFav.X = 65.0;
					goto IL_0294;
					IL_0294:
					num2 = 46;
					pFav.Y = 50.0;
					goto IL_02a7;
					IL_02a7:
					num2 = 47;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_02b8:
					num2 = 49;
					if (Operators.ConditionalCompareObjectEqual(left, "StylographBlue", TextCompare: false))
					{
						goto IL_02ca;
					}
					goto IL_0333;
					IL_02ca:
					num2 = 50;
					pFav.X = 85.0;
					goto IL_02dd;
					IL_02dd:
					num2 = 51;
					pFav.Y = 20.0;
					goto IL_02f0;
					IL_02f0:
					num2 = 52;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_02fc;
					IL_02fc:
					num2 = 53;
					pFav.X = 65.0;
					goto IL_030f;
					IL_030f:
					num2 = 54;
					pFav.Y = 50.0;
					goto IL_0322;
					IL_0322:
					num2 = 55;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0333:
					num2 = 57;
					if (Operators.ConditionalCompareObjectEqual(left, "HighlighterBlue", TextCompare: false))
					{
						goto IL_0345;
					}
					goto IL_03ae;
					IL_0345:
					num2 = 58;
					pFav.X = 130.0;
					goto IL_0358;
					IL_0358:
					num2 = 59;
					pFav.Y = 20.0;
					goto IL_036b;
					IL_036b:
					num2 = 60;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0377;
					IL_0377:
					num2 = 61;
					pFav.X = 65.0;
					goto IL_038a;
					IL_038a:
					num2 = 62;
					pFav.Y = 50.0;
					goto IL_039d;
					IL_039d:
					num2 = 63;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_03ae:
					num2 = 65;
					if (Operators.ConditionalCompareObjectEqual(left, "MarkerGreen", TextCompare: false))
					{
						goto IL_03c0;
					}
					goto IL_0429;
					IL_03c0:
					num2 = 66;
					pFav.X = 40.0;
					goto IL_03d3;
					IL_03d3:
					num2 = 67;
					pFav.Y = 20.0;
					goto IL_03e6;
					IL_03e6:
					num2 = 68;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_03f2;
					IL_03f2:
					num2 = 69;
					pFav.X = 105.0;
					goto IL_0405;
					IL_0405:
					num2 = 70;
					pFav.Y = 50.0;
					goto IL_0418;
					IL_0418:
					num2 = 71;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0429:
					num2 = 73;
					if (Operators.ConditionalCompareObjectEqual(left, "StylographGreen", TextCompare: false))
					{
						goto IL_043b;
					}
					goto IL_04a4;
					IL_043b:
					num2 = 74;
					pFav.X = 85.0;
					goto IL_044e;
					IL_044e:
					num2 = 75;
					pFav.Y = 20.0;
					goto IL_0461;
					IL_0461:
					num2 = 76;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_046d;
					IL_046d:
					num2 = 77;
					pFav.X = 105.0;
					goto IL_0480;
					IL_0480:
					num2 = 78;
					pFav.Y = 50.0;
					goto IL_0493;
					IL_0493:
					num2 = 79;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_04a4:
					num2 = 81;
					if (Operators.ConditionalCompareObjectEqual(left, "HighlighterGreen", TextCompare: false))
					{
						goto IL_04b6;
					}
					goto IL_051f;
					IL_04b6:
					num2 = 82;
					pFav.X = 130.0;
					goto IL_04c9;
					IL_04c9:
					num2 = 83;
					pFav.Y = 20.0;
					goto IL_04dc;
					IL_04dc:
					num2 = 84;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_04e8;
					IL_04e8:
					num2 = 85;
					pFav.X = 105.0;
					goto IL_04fb;
					IL_04fb:
					num2 = 86;
					pFav.Y = 50.0;
					goto IL_050e;
					IL_050e:
					num2 = 87;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_051f:
					num2 = 89;
					if (Operators.ConditionalCompareObjectEqual(left, "MarkerOrange", TextCompare: false))
					{
						goto IL_0531;
					}
					goto IL_059a;
					IL_0531:
					num2 = 90;
					pFav.X = 40.0;
					goto IL_0544;
					IL_0544:
					num2 = 91;
					pFav.Y = 20.0;
					goto IL_0557;
					IL_0557:
					num2 = 92;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0563;
					IL_0563:
					num2 = 93;
					pFav.X = 135.0;
					goto IL_0576;
					IL_0576:
					num2 = 94;
					pFav.Y = 50.0;
					goto IL_0589;
					IL_0589:
					num2 = 95;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_059a:
					num2 = 97;
					if (Operators.ConditionalCompareObjectEqual(left, "StylographOrange", TextCompare: false))
					{
						goto IL_05ac;
					}
					goto IL_0615;
					IL_05ac:
					num2 = 98;
					pFav.X = 85.0;
					goto IL_05bf;
					IL_05bf:
					num2 = 99;
					pFav.Y = 20.0;
					goto IL_05d2;
					IL_05d2:
					num2 = 100;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_05de;
					IL_05de:
					num2 = 101;
					pFav.X = 135.0;
					goto IL_05f1;
					IL_05f1:
					num2 = 102;
					pFav.Y = 50.0;
					goto IL_0604;
					IL_0604:
					num2 = 103;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0615:
					num2 = 105;
					if (Operators.ConditionalCompareObjectEqual(left, "HighlighterYellow", TextCompare: false))
					{
						goto IL_0627;
					}
					goto IL_0690;
					IL_0627:
					num2 = 106;
					pFav.X = 130.0;
					goto IL_063a;
					IL_063a:
					num2 = 107;
					pFav.Y = 20.0;
					goto IL_064d;
					IL_064d:
					num2 = 108;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0659;
					IL_0659:
					num2 = 109;
					pFav.X = 135.0;
					goto IL_066c;
					IL_066c:
					num2 = 110;
					pFav.Y = 50.0;
					goto IL_067f;
					IL_067f:
					num2 = 111;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0690:
					num2 = 113;
					if (Operators.ConditionalCompareObjectEqual(left, "MarkerBlack", TextCompare: false))
					{
						goto IL_06a2;
					}
					goto IL_070b;
					IL_06a2:
					num2 = 114;
					pFav.X = 40.0;
					goto IL_06b5;
					IL_06b5:
					num2 = 115;
					pFav.Y = 20.0;
					goto IL_06c8;
					IL_06c8:
					num2 = 116;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_06d4;
					IL_06d4:
					num2 = 117;
					pFav.X = 35.0;
					goto IL_06e7;
					IL_06e7:
					num2 = 118;
					pFav.Y = 85.0;
					goto IL_06fa;
					IL_06fa:
					num2 = 119;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_070b:
					num2 = 121;
					if (Operators.ConditionalCompareObjectEqual(left, "StylographBlack", TextCompare: false))
					{
						goto IL_071d;
					}
					goto IL_0786;
					IL_071d:
					num2 = 122;
					pFav.X = 85.0;
					goto IL_0730;
					IL_0730:
					num2 = 123;
					pFav.Y = 20.0;
					goto IL_0743;
					IL_0743:
					num2 = 124;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_074f;
					IL_074f:
					num2 = 125;
					pFav.X = 35.0;
					goto IL_0762;
					IL_0762:
					num2 = 126;
					pFav.Y = 85.0;
					goto IL_0775;
					IL_0775:
					num2 = 127;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0786:
					num2 = 129;
					if (Operators.ConditionalCompareObjectEqual(left, "HighlighterBlack", TextCompare: false))
					{
						goto IL_079b;
					}
					goto IL_0816;
					IL_079b:
					num2 = 130;
					pFav.X = 130.0;
					goto IL_07b1;
					IL_07b1:
					num2 = 131;
					pFav.Y = 20.0;
					goto IL_07c7;
					IL_07c7:
					num2 = 132;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_07d6;
					IL_07d6:
					num2 = 133;
					pFav.X = 35.0;
					goto IL_07ec;
					IL_07ec:
					num2 = 134;
					pFav.Y = 85.0;
					goto IL_0802;
					IL_0802:
					num2 = 135;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0816:
					num2 = 137;
					if (Operators.ConditionalCompareObjectEqual(left, "MarkerWhite", TextCompare: false))
					{
						goto IL_082b;
					}
					goto IL_08a6;
					IL_082b:
					num2 = 138;
					pFav.X = 40.0;
					goto IL_0841;
					IL_0841:
					num2 = 139;
					pFav.Y = 20.0;
					goto IL_0857;
					IL_0857:
					num2 = 140;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0866;
					IL_0866:
					num2 = 141;
					pFav.X = 65.0;
					goto IL_087c;
					IL_087c:
					num2 = 142;
					pFav.Y = 85.0;
					goto IL_0892;
					IL_0892:
					num2 = 143;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_08a6:
					num2 = 145;
					if (Operators.ConditionalCompareObjectEqual(left, "StylographWhite", TextCompare: false))
					{
						goto IL_08bb;
					}
					goto IL_0936;
					IL_08bb:
					num2 = 146;
					pFav.X = 85.0;
					goto IL_08d1;
					IL_08d1:
					num2 = 147;
					pFav.Y = 20.0;
					goto IL_08e7;
					IL_08e7:
					num2 = 148;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_08f6;
					IL_08f6:
					num2 = 149;
					pFav.X = 65.0;
					goto IL_090c;
					IL_090c:
					num2 = 150;
					pFav.Y = 85.0;
					goto IL_0922;
					IL_0922:
					num2 = 151;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0936:
					num2 = 153;
					if (Operators.ConditionalCompareObjectEqual(left, "HighlighterWhite", TextCompare: false))
					{
						goto IL_094b;
					}
					goto IL_09c6;
					IL_094b:
					num2 = 154;
					pFav.X = 130.0;
					goto IL_0961;
					IL_0961:
					num2 = 155;
					pFav.Y = 20.0;
					goto IL_0977;
					IL_0977:
					num2 = 156;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0986;
					IL_0986:
					num2 = 157;
					pFav.X = 65.0;
					goto IL_099c;
					IL_099c:
					num2 = 158;
					pFav.Y = 85.0;
					goto IL_09b2;
					IL_09b2:
					num2 = 159;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_09c6:
					num2 = 161;
					if (Operators.ConditionalCompareObjectEqual(left, "MarkerColorFul", TextCompare: false))
					{
						goto IL_09db;
					}
					goto IL_0a56;
					IL_09db:
					num2 = 162;
					pFav.X = 40.0;
					goto IL_09f1;
					IL_09f1:
					num2 = 163;
					pFav.Y = 20.0;
					goto IL_0a07;
					IL_0a07:
					num2 = 164;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0a16;
					IL_0a16:
					num2 = 165;
					pFav.X = 105.0;
					goto IL_0a2c;
					IL_0a2c:
					num2 = 166;
					pFav.Y = 85.0;
					goto IL_0a42;
					IL_0a42:
					num2 = 167;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0a56:
					num2 = 169;
					if (Operators.ConditionalCompareObjectEqual(left, "StylographColorFul", TextCompare: false))
					{
						goto IL_0a6b;
					}
					goto IL_0ae6;
					IL_0a6b:
					num2 = 170;
					pFav.X = 85.0;
					goto IL_0a81;
					IL_0a81:
					num2 = 171;
					pFav.Y = 20.0;
					goto IL_0a97;
					IL_0a97:
					num2 = 172;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0aa6;
					IL_0aa6:
					num2 = 173;
					pFav.X = 105.0;
					goto IL_0abc;
					IL_0abc:
					num2 = 174;
					pFav.Y = 85.0;
					goto IL_0ad2;
					IL_0ad2:
					num2 = 175;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0ae6:
					num2 = 177;
					if (Operators.ConditionalCompareObjectEqual(left, "HighlighterColorFul", TextCompare: false))
					{
						goto IL_0afb;
					}
					goto IL_0b76;
					IL_0afb:
					num2 = 178;
					pFav.X = 130.0;
					goto IL_0b11;
					IL_0b11:
					num2 = 179;
					pFav.Y = 20.0;
					goto IL_0b27;
					IL_0b27:
					num2 = 180;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto IL_0b36;
					IL_0b36:
					num2 = 181;
					pFav.X = 105.0;
					goto IL_0b4c;
					IL_0b4c:
					num2 = 182;
					pFav.Y = 85.0;
					goto IL_0b62;
					IL_0b62:
					num2 = 183;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0b76:
					num2 = 185;
					if (Operators.ConditionalCompareObjectEqual(left, "MarkerNoPen", TextCompare: false))
					{
						goto IL_0b8b;
					}
					goto IL_0bcb;
					IL_0b8b:
					num2 = 186;
					pFav.X = 135.0;
					goto IL_0ba1;
					IL_0ba1:
					num2 = 187;
					pFav.Y = 85.0;
					goto IL_0bb7;
					IL_0bb7:
					num2 = 188;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0bcb:
					num2 = 190;
					if (Operators.ConditionalCompareObjectEqual(left, "StylographNoPen", TextCompare: false))
					{
						goto IL_0be0;
					}
					goto IL_0c20;
					IL_0be0:
					num2 = 191;
					pFav.X = 135.0;
					goto IL_0bf6;
					IL_0bf6:
					num2 = 192;
					pFav.Y = 85.0;
					goto IL_0c0c;
					IL_0c0c:
					num2 = 193;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0c20:
					num2 = 195;
					if (Operators.ConditionalCompareObjectEqual(left, "HighlighterNoPen", TextCompare: false))
					{
						goto IL_0c35;
					}
					goto IL_0c75;
					IL_0c35:
					num2 = 196;
					pFav.X = 135.0;
					goto IL_0c4b;
					IL_0c4b:
					num2 = 197;
					pFav.Y = 85.0;
					goto IL_0c61;
					IL_0c61:
					num2 = 198;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0c75:
					num2 = 200;
					if (Operators.ConditionalCompareObjectEqual(left, "Size1", TextCompare: false))
					{
						goto IL_0c8a;
					}
					goto IL_0cca;
					IL_0c8a:
					num2 = 201;
					pFav.X = 22.0;
					goto IL_0ca0;
					IL_0ca0:
					num2 = 202;
					pFav.Y = 120.0;
					goto IL_0cb6;
					IL_0cb6:
					num2 = 203;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0cca:
					num2 = 205;
					if (Operators.ConditionalCompareObjectEqual(left, "Size2", TextCompare: false))
					{
						goto IL_0cdf;
					}
					goto IL_0d1f;
					IL_0cdf:
					num2 = 206;
					pFav.X = 40.0;
					goto IL_0cf5;
					IL_0cf5:
					num2 = 207;
					pFav.Y = 120.0;
					goto IL_0d0b;
					IL_0d0b:
					num2 = 208;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0d1f:
					num2 = 210;
					if (Operators.ConditionalCompareObjectEqual(left, "Size3", TextCompare: false))
					{
						goto IL_0d34;
					}
					goto IL_0d74;
					IL_0d34:
					num2 = 211;
					pFav.X = 57.0;
					goto IL_0d4a;
					IL_0d4a:
					num2 = 212;
					pFav.Y = 120.0;
					goto IL_0d60;
					IL_0d60:
					num2 = 213;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0d74:
					num2 = 215;
					if (Operators.ConditionalCompareObjectEqual(left, "Size4", TextCompare: false))
					{
						goto IL_0d89;
					}
					goto IL_0dc9;
					IL_0d89:
					num2 = 216;
					pFav.X = 82.0;
					goto IL_0d9f;
					IL_0d9f:
					num2 = 217;
					pFav.Y = 120.0;
					goto IL_0db5;
					IL_0db5:
					num2 = 218;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0dc9:
					num2 = 220;
					if (Operators.ConditionalCompareObjectEqual(left, "Size5", TextCompare: false))
					{
						goto IL_0dde;
					}
					goto IL_0e1e;
					IL_0dde:
					num2 = 221;
					pFav.X = 105.0;
					goto IL_0df4;
					IL_0df4:
					num2 = 222;
					pFav.Y = 120.0;
					goto IL_0e0a;
					IL_0e0a:
					num2 = 223;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0e1e:
					num2 = 225;
					if (Operators.ConditionalCompareObjectEqual(left, "Size6", TextCompare: false))
					{
						goto IL_0e33;
					}
					goto IL_0e73;
					IL_0e33:
					num2 = 226;
					pFav.X = 135.0;
					goto IL_0e49;
					IL_0e49:
					num2 = 227;
					pFav.Y = 120.0;
					goto IL_0e5f;
					IL_0e5f:
					num2 = 228;
					SubMenuPen_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0e73:
					num2 = 230;
					if (Operators.ConditionalCompareObjectEqual(left, "Curtain", TextCompare: false))
					{
						goto IL_0e88;
					}
					goto IL_0ec8;
					IL_0e88:
					num2 = 231;
					pFav.X = 130.0;
					goto IL_0e9e;
					IL_0e9e:
					num2 = 232;
					pFav.Y = 30.0;
					goto IL_0eb4;
					IL_0eb4:
					num2 = 233;
					SubMenuEraser_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0ec8:
					num2 = 235;
					if (Operators.ConditionalCompareObjectEqual(left, "Eraser1", TextCompare: false))
					{
						goto IL_0edd;
					}
					goto IL_0f1d;
					IL_0edd:
					num2 = 236;
					pFav.X = 35.0;
					goto IL_0ef3;
					IL_0ef3:
					num2 = 237;
					pFav.Y = 65.0;
					goto IL_0f09;
					IL_0f09:
					num2 = 238;
					SubMenuEraser_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0f1d:
					num2 = 240;
					if (Operators.ConditionalCompareObjectEqual(left, "Eraser2", TextCompare: false))
					{
						goto IL_0f32;
					}
					goto IL_0f72;
					IL_0f32:
					num2 = 241;
					pFav.X = 60.0;
					goto IL_0f48;
					IL_0f48:
					num2 = 242;
					pFav.Y = 65.0;
					goto IL_0f5e;
					IL_0f5e:
					num2 = 243;
					SubMenuEraser_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0f72:
					num2 = 245;
					if (Operators.ConditionalCompareObjectEqual(left, "Eraser3", TextCompare: false))
					{
						goto IL_0f87;
					}
					goto IL_0fc7;
					IL_0f87:
					num2 = 246;
					pFav.X = 93.0;
					goto IL_0f9d;
					IL_0f9d:
					num2 = 247;
					pFav.Y = 65.0;
					goto IL_0fb3;
					IL_0fb3:
					num2 = 248;
					SubMenuEraser_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_0fc7:
					num2 = 250;
					if (Operators.ConditionalCompareObjectEqual(left, "Eraser4", TextCompare: false))
					{
						goto IL_0fdc;
					}
					goto IL_101c;
					IL_0fdc:
					num2 = 251;
					pFav.X = 129.0;
					goto IL_0ff2;
					IL_0ff2:
					num2 = 252;
					pFav.Y = 65.0;
					goto IL_1008;
					IL_1008:
					num2 = 253;
					SubMenuEraser_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_101c:
					num2 = 255;
					if (Operators.ConditionalCompareObjectEqual(left, "Line", TextCompare: false))
					{
						goto IL_1031;
					}
					goto IL_1071;
					IL_1031:
					num2 = 256;
					pFav.X = 40.0;
					goto IL_1047;
					IL_1047:
					num2 = 257;
					pFav.Y = 30.0;
					goto IL_105d;
					IL_105d:
					num2 = 258;
					SubMenuShape_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_1071:
					num2 = 260;
					if (Operators.ConditionalCompareObjectEqual(left, "DashLine", TextCompare: false))
					{
						goto IL_1086;
					}
					goto IL_10c6;
					IL_1086:
					num2 = 261;
					pFav.X = 80.0;
					goto IL_109c;
					IL_109c:
					num2 = 262;
					pFav.Y = 30.0;
					goto IL_10b2;
					IL_10b2:
					num2 = 263;
					SubMenuShape_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_10c6:
					num2 = 265;
					if (Operators.ConditionalCompareObjectEqual(left, "Arrow", TextCompare: false))
					{
						goto IL_10db;
					}
					goto IL_111b;
					IL_10db:
					num2 = 266;
					pFav.X = 125.0;
					goto IL_10f1;
					IL_10f1:
					num2 = 267;
					pFav.Y = 30.0;
					goto IL_1107;
					IL_1107:
					num2 = 268;
					SubMenuShape_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_111b:
					num2 = 270;
					if (Operators.ConditionalCompareObjectEqual(left, "Rectangle", TextCompare: false))
					{
						goto IL_1130;
					}
					goto IL_1170;
					IL_1130:
					num2 = 271;
					pFav.X = 40.0;
					goto IL_1146;
					IL_1146:
					num2 = 272;
					pFav.Y = 60.0;
					goto IL_115c;
					IL_115c:
					num2 = 273;
					SubMenuShape_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_1170:
					num2 = 275;
					if (Operators.ConditionalCompareObjectEqual(left, "Ellipse", TextCompare: false))
					{
						goto IL_1185;
					}
					goto IL_11c5;
					IL_1185:
					num2 = 276;
					pFav.X = 80.0;
					goto IL_119b;
					IL_119b:
					num2 = 277;
					pFav.Y = 60.0;
					goto IL_11b1;
					IL_11b1:
					num2 = 278;
					SubMenuShape_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_11c5:
					num2 = 280;
					if (Operators.ConditionalCompareObjectEqual(left, "Triangle", TextCompare: false))
					{
						goto IL_11da;
					}
					goto IL_121a;
					IL_11da:
					num2 = 281;
					pFav.X = 125.0;
					goto IL_11f0;
					IL_11f0:
					num2 = 282;
					pFav.Y = 60.0;
					goto IL_1206;
					IL_1206:
					num2 = 283;
					SubMenuShape_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_121a:
					num2 = 285;
					if (Operators.ConditionalCompareObjectEqual(left, "Library", TextCompare: false))
					{
						goto IL_122f;
					}
					goto IL_126c;
					IL_122f:
					num2 = 286;
					pFav.X = 50.0;
					goto IL_1245;
					IL_1245:
					num2 = 287;
					pFav.Y = 95.0;
					goto IL_125b;
					IL_125b:
					num2 = 288;
					SubMenuShape_MouseLeftButtonUp(null, null, pFav);
					goto end_IL_0000_3;
					IL_126c:
					num2 = 290;
					if (!Operators.ConditionalCompareObjectEqual(left, "Background", TextCompare: false))
					{
						goto end_IL_0000_3;
					}
					goto IL_1281;
					IL_1281:
					num2 = 291;
					pFav.X = 110.0;
					goto IL_1297;
					end_IL_0000_2:
					break;
				}
				num2 = 293;
				SubMenuShape_MouseLeftButtonUp(null, null, pFav);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 5993;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void LoadFavourites()
	{
		int try0000_dispatch = -1;
		int num2 = default(int);
		int num = default(int);
		int num3 = default(int);
		byte b = default(byte);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
					num2 = 1;
					if (IgnoreErrors)
					{
						goto IL_000a;
					}
					goto IL_0011;
				case 1103:
					{
						num = num2;
						switch (num3)
						{
						case 1:
							break;
						default:
							goto end_IL_0000;
						}
						int num4 = num + 1;
						num = 0;
						switch (num4)
						{
						case 1:
							break;
						case 2:
							goto IL_000a;
						case 3:
							goto IL_0011;
						case 4:
							goto IL_001f;
						case 5:
							goto IL_0028;
						case 6:
							goto IL_0031;
						case 7:
							goto IL_0037;
						case 8:
							goto IL_0059;
						case 9:
							goto IL_005f;
						case 10:
							goto IL_0082;
						case 11:
							goto IL_0089;
						case 12:
							goto IL_00ac;
						case 13:
							goto IL_00b3;
						case 14:
							goto IL_00d6;
						case 15:
							goto IL_00dd;
						case 16:
							goto IL_0100;
						case 17:
							goto IL_0107;
						case 18:
							goto IL_012a;
						case 19:
							goto IL_0131;
						case 20:
							goto IL_0154;
						case 21:
							goto IL_015b;
						case 22:
							goto IL_017e;
						case 23:
							goto IL_0186;
						case 24:
							goto IL_01a9;
						case 25:
							goto IL_01b1;
						case 26:
							goto IL_01d4;
						case 27:
							goto IL_01dc;
						case 28:
							goto IL_01ff;
						case 29:
							goto IL_0207;
						case 30:
							goto IL_022a;
						case 31:
							goto IL_0232;
						case 32:
							goto IL_0255;
						case 33:
							goto IL_025d;
						case 34:
							goto IL_0280;
						case 35:
							goto IL_0288;
						case 36:
							goto IL_02ab;
						case 37:
							goto IL_02b3;
						case 38:
							goto IL_02d6;
						case 39:
							goto IL_02de;
						case 40:
							goto IL_0301;
						case 41:
							goto IL_0309;
						case 42:
							goto IL_032c;
						case 43:
							goto IL_0334;
						case 44:
							goto IL_0357;
						case 45:
							goto end_IL_0000_2;
						default:
							goto end_IL_0000;
						case 46:
							goto end_IL_0000_3;
						}
						goto default;
					}
					IL_0357:
					num2 = 44;
					if (b != 20)
					{
						goto end_IL_0000_3;
					}
					break;
					IL_000a:
					ProjectData.ClearProjectError();
					num3 = 1;
					goto IL_0011;
					IL_0011:
					num2 = 3;
					if (iFav <= 0)
					{
						goto end_IL_0000_3;
					}
					goto IL_001f;
					IL_001f:
					num2 = 4;
					b = iFav;
					goto IL_0028;
					IL_0028:
					num2 = 5;
					iFav = 0;
					goto IL_0031;
					IL_0031:
					num2 = 6;
					if (b >= 1)
					{
						goto IL_0037;
					}
					goto IL_0059;
					IL_0037:
					num2 = 7;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav1), MySettingsProperty.Settings.Fav1);
					goto IL_0059;
					IL_0059:
					num2 = 8;
					if (b >= 2)
					{
						goto IL_005f;
					}
					goto IL_0082;
					IL_005f:
					num2 = 9;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav2), MySettingsProperty.Settings.Fav2);
					goto IL_0082;
					IL_0082:
					num2 = 10;
					if (b >= 3)
					{
						goto IL_0089;
					}
					goto IL_00ac;
					IL_0089:
					num2 = 11;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav3), MySettingsProperty.Settings.Fav3);
					goto IL_00ac;
					IL_00ac:
					num2 = 12;
					if (b >= 4)
					{
						goto IL_00b3;
					}
					goto IL_00d6;
					IL_00b3:
					num2 = 13;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav4), MySettingsProperty.Settings.Fav4);
					goto IL_00d6;
					IL_00d6:
					num2 = 14;
					if (b >= 5)
					{
						goto IL_00dd;
					}
					goto IL_0100;
					IL_00dd:
					num2 = 15;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav5), MySettingsProperty.Settings.Fav5);
					goto IL_0100;
					IL_0100:
					num2 = 16;
					if (b >= 6)
					{
						goto IL_0107;
					}
					goto IL_012a;
					IL_0107:
					num2 = 17;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav6), MySettingsProperty.Settings.Fav6);
					goto IL_012a;
					IL_012a:
					num2 = 18;
					if (b >= 7)
					{
						goto IL_0131;
					}
					goto IL_0154;
					IL_0131:
					num2 = 19;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav7), MySettingsProperty.Settings.Fav7);
					goto IL_0154;
					IL_0154:
					num2 = 20;
					if (b >= 8)
					{
						goto IL_015b;
					}
					goto IL_017e;
					IL_015b:
					num2 = 21;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav8), MySettingsProperty.Settings.Fav8);
					goto IL_017e;
					IL_017e:
					num2 = 22;
					if (b >= 9)
					{
						goto IL_0186;
					}
					goto IL_01a9;
					IL_0186:
					num2 = 23;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav9), MySettingsProperty.Settings.Fav9);
					goto IL_01a9;
					IL_01a9:
					num2 = 24;
					if (b >= 10)
					{
						goto IL_01b1;
					}
					goto IL_01d4;
					IL_01b1:
					num2 = 25;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav10), MySettingsProperty.Settings.Fav10);
					goto IL_01d4;
					IL_01d4:
					num2 = 26;
					if (b >= 11)
					{
						goto IL_01dc;
					}
					goto IL_01ff;
					IL_01dc:
					num2 = 27;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav11), MySettingsProperty.Settings.Fav11);
					goto IL_01ff;
					IL_01ff:
					num2 = 28;
					if (b >= 12)
					{
						goto IL_0207;
					}
					goto IL_022a;
					IL_0207:
					num2 = 29;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav12), MySettingsProperty.Settings.Fav12);
					goto IL_022a;
					IL_022a:
					num2 = 30;
					if (b >= 13)
					{
						goto IL_0232;
					}
					goto IL_0255;
					IL_0232:
					num2 = 31;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav13), MySettingsProperty.Settings.Fav13);
					goto IL_0255;
					IL_0255:
					num2 = 32;
					if (b >= 14)
					{
						goto IL_025d;
					}
					goto IL_0280;
					IL_025d:
					num2 = 33;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav14), MySettingsProperty.Settings.Fav14);
					goto IL_0280;
					IL_0280:
					num2 = 34;
					if (b >= 15)
					{
						goto IL_0288;
					}
					goto IL_02ab;
					IL_0288:
					num2 = 35;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav15), MySettingsProperty.Settings.Fav15);
					goto IL_02ab;
					IL_02ab:
					num2 = 36;
					if (b >= 16)
					{
						goto IL_02b3;
					}
					goto IL_02d6;
					IL_02b3:
					num2 = 37;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav16), MySettingsProperty.Settings.Fav16);
					goto IL_02d6;
					IL_02d6:
					num2 = 38;
					if (b >= 17)
					{
						goto IL_02de;
					}
					goto IL_0301;
					IL_02de:
					num2 = 39;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav17), MySettingsProperty.Settings.Fav17);
					goto IL_0301;
					IL_0301:
					num2 = 40;
					if (b >= 18)
					{
						goto IL_0309;
					}
					goto IL_032c;
					IL_0309:
					num2 = 41;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav18), MySettingsProperty.Settings.Fav18);
					goto IL_032c;
					IL_032c:
					num2 = 42;
					if (b >= 19)
					{
						goto IL_0334;
					}
					goto IL_0357;
					IL_0334:
					num2 = 43;
					SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav19), MySettingsProperty.Settings.Fav19);
					goto IL_0357;
					end_IL_0000_2:
					break;
				}
				num2 = 45;
				SetFavIcon(SelectFavImg(MySettingsProperty.Settings.Fav20), MySettingsProperty.Settings.Fav20);
				break;
				end_IL_0000:;
			}
			catch (Exception) when (num3 != 0 && num == 0)
			{
				// Error handled by On Error Resume Next
				try0000_dispatch = 1103;
				continue;
			}
			throw ProjectData.CreateProjectError(-2146828237);
			continue;
			end_IL_0000_3:
			break;
		}
		if (num != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private Image SelectFavImg(string txtTag)
	{
		switch (txtTag)
		{
		case "Undo":
			if (FrameNo == 0L)
			{
				return imgFavGrayedUndo;
			}
			return imgFavUndo;
		case "Redo":
			if (iRedo == 0)
			{
				return imgFavGrayedRedo;
			}
			return imgFavRedo;
		case "MarkerRed":
			return imgFavMarkerRed;
		case "StylographRed":
			return imgFavStylographRed;
		case "HighlighterRed":
			return imgFavHighlighterRed;
		case "MarkerBlue":
			return imgFavMarkerBlue;
		case "StylographBlue":
			return imgFavStylographBlue;
		case "HighlighterBlue":
			return imgFavHighlighterBlue;
		case "MarkerGreen":
			return imgFavMarkerGreen;
		case "StylographGreen":
			return imgFavStylographGreen;
		case "HighlighterGreen":
			return imgFavHighlighterGreen;
		case "MarkerOrange":
			return imgFavMarkerOrange;
		case "StylographOrange":
			return imgFavStylographOrange;
		case "HighlighterYellow":
			return imgFavHighlighterYellow;
		case "MarkerBlack":
			return imgFavMarkerBlack;
		case "StylographBlack":
			return imgFavStylographBlack;
		case "HighlighterBlack":
			return imgFavHighlighterBlack;
		case "MarkerWhite":
			return imgFavMarkerWhite;
		case "StylographWhite":
			return imgFavStylographWhite;
		case "HighlighterWhite":
			return imgFavHighlighterWhite;
		case "MarkerColorFul":
			return imgFavMarkerColorFul;
		case "StylographColorFul":
			return imgFavStylographColorful;
		case "HighlighterColorFul":
			return imgFavHighlighterColorful;
		case "MarkerNoPen":
			return imgFavMarkerNoPen;
		case "StylographNoPen":
			return imgFavStylographNoPen;
		case "HighlighterNoPen":
			return imgFavHighlighterNoPen;
		case "Size1":
			return imgFavSize1;
		case "Size2":
			return imgFavSize2;
		case "Size3":
			return imgFavSize3;
		case "Size4":
			return imgFavSize4;
		case "Size5":
			return imgFavSize5;
		case "Size6":
			return imgFavSize6;
		case "Curtain":
			return imgFavCurtain;
		case "Eraser1":
			return imgFavEraser1;
		case "Eraser2":
			return imgFavEraser2;
		case "Eraser3":
			return imgFavEraser3;
		case "Eraser4":
			return imgFavEraser4;
		case "Line":
			return imgFavLine;
		case "DashLine":
			return imgFavDashLine;
		case "Arrow":
			return imgFavArrow;
		case "Rectangle":
			return imgFavRectangle;
		case "Ellipse":
			return imgFavEllipse;
		case "Triangle":
			return imgFavTriangle;
		case "Library":
			return imgFavLibrary;
		case "Background":
			return imgFavBackground;
		default:
			return null;
		}
	}

	private void tmrDebug_Tick(object sender, EventArgs e)
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/Fatih Kalem;component/mainwindow.xaml", UriKind.Relative);
			System.Windows.Application.LoadComponent(this, resourceLocator);
		}
	}

	void IComponentConnector.InitializeComponent()
	{
		//ILSpy generated this explicit interface implementation from .override directive in InitializeComponent
		this.InitializeComponent();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
	[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
	[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
	public void System_Windows_Markup_IComponentConnector_Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			borderImages = (Border)target;
			break;
		case 2:
			imgHandRes = (Image)target;
			break;
		case 3:
			imgPenDrawing = (Image)target;
			break;
		case 4:
			imgPenRed = (Image)target;
			break;
		case 5:
			imgPenBlue = (Image)target;
			break;
		case 6:
			imgPenGreen = (Image)target;
			break;
		case 7:
			imgPenOrange = (Image)target;
			break;
		case 8:
			imgPenBlack = (Image)target;
			break;
		case 9:
			imgPenWhite = (Image)target;
			break;
		case 10:
			imgPenColorful = (Image)target;
			break;
		case 11:
			imgNoPen = (Image)target;
			break;
		case 12:
			imgStylographRed = (Image)target;
			break;
		case 13:
			imgStylographBlue = (Image)target;
			break;
		case 14:
			imgStylographGreen = (Image)target;
			break;
		case 15:
			imgStylographOrange = (Image)target;
			break;
		case 16:
			imgStylographBlack = (Image)target;
			break;
		case 17:
			imgStylographWhite = (Image)target;
			break;
		case 18:
			imgStylographColorful = (Image)target;
			break;
		case 19:
			imgStylographNoPen = (Image)target;
			break;
		case 20:
			imgHighlighterRed = (Image)target;
			break;
		case 21:
			imgHighlighterBlue = (Image)target;
			break;
		case 22:
			imgHighlighterGreen = (Image)target;
			break;
		case 23:
			imgHighlighterOrange = (Image)target;
			break;
		case 24:
			imgHighlighterBlack = (Image)target;
			break;
		case 25:
			imgHighlighterWhite = (Image)target;
			break;
		case 26:
			imgHighlighterColorful = (Image)target;
			break;
		case 27:
			imgHighlighterNoPen = (Image)target;
			break;
		case 28:
			imgSubMenuMarkerRight = (Image)target;
			break;
		case 29:
			imgSubMenuMarkerLeft = (Image)target;
			break;
		case 30:
			imgSubMenuStylographRight = (Image)target;
			break;
		case 31:
			imgSubMenuStylographLeft = (Image)target;
			break;
		case 32:
			imgSubMenuHighlighterRight = (Image)target;
			break;
		case 33:
			imgSubMenuHighlighterLeft = (Image)target;
			break;
		case 34:
			imgGestureEraser = (Image)target;
			break;
		case 35:
			imgGestureMarkerRed = (Image)target;
			break;
		case 36:
			imgGestureMarkerBlue = (Image)target;
			break;
		case 37:
			imgGestureMarkerGreen = (Image)target;
			break;
		case 38:
			imgGestureMarkerBlack = (Image)target;
			break;
		case 39:
			imgGestureStylographRed = (Image)target;
			break;
		case 40:
			imgGestureStylographBlue = (Image)target;
			break;
		case 41:
			imgGestureStylographGreen = (Image)target;
			break;
		case 42:
			imgGestureStylographBlack = (Image)target;
			break;
		case 43:
			imgGestureHighlighterRed = (Image)target;
			break;
		case 44:
			imgGestureHighlighterBlue = (Image)target;
			break;
		case 45:
			imgGestureHighlighterGreen = (Image)target;
			break;
		case 46:
			imgGestureHighlighterYellow = (Image)target;
			break;
		case 47:
			imgFavArrow = (Image)target;
			break;
		case 48:
			imgFavBackground = (Image)target;
			break;
		case 49:
			imgFavCurtain = (Image)target;
			break;
		case 50:
			imgFavDashLine = (Image)target;
			break;
		case 51:
			imgFavEllipse = (Image)target;
			break;
		case 52:
			imgFavEraser1 = (Image)target;
			break;
		case 53:
			imgFavEraser2 = (Image)target;
			break;
		case 54:
			imgFavEraser3 = (Image)target;
			break;
		case 55:
			imgFavEraser4 = (Image)target;
			break;
		case 56:
			imgFavGrayedRedo = (Image)target;
			break;
		case 57:
			imgFavGrayedUndo = (Image)target;
			break;
		case 58:
			imgFavHighlighterBlack = (Image)target;
			break;
		case 59:
			imgFavHighlighterBlue = (Image)target;
			break;
		case 60:
			imgFavHighlighterColorful = (Image)target;
			break;
		case 61:
			imgFavHighlighterGreen = (Image)target;
			break;
		case 62:
			imgFavHighlighterNoPen = (Image)target;
			break;
		case 63:
			imgFavHighlighterRed = (Image)target;
			break;
		case 64:
			imgFavHighlighterWhite = (Image)target;
			break;
		case 65:
			imgFavHighlighterYellow = (Image)target;
			break;
		case 66:
			imgFavLibrary = (Image)target;
			break;
		case 67:
			imgFavLine = (Image)target;
			break;
		case 68:
			imgFavMarkerBlack = (Image)target;
			break;
		case 69:
			imgFavMarkerBlue = (Image)target;
			break;
		case 70:
			imgFavMarkerColorFul = (Image)target;
			break;
		case 71:
			imgFavMarkerGreen = (Image)target;
			break;
		case 72:
			imgFavMarkerNoPen = (Image)target;
			break;
		case 73:
			imgFavMarkerOrange = (Image)target;
			break;
		case 74:
			imgFavMarkerRed = (Image)target;
			break;
		case 75:
			imgFavMarkerWhite = (Image)target;
			break;
		case 76:
			imgFavRectangle = (Image)target;
			break;
		case 77:
			imgFavRedo = (Image)target;
			break;
		case 78:
			imgFavSize1 = (Image)target;
			break;
		case 79:
			imgFavSize2 = (Image)target;
			break;
		case 80:
			imgFavSize3 = (Image)target;
			break;
		case 81:
			imgFavSize4 = (Image)target;
			break;
		case 82:
			imgFavSize5 = (Image)target;
			break;
		case 83:
			imgFavSize6 = (Image)target;
			break;
		case 84:
			imgFavStylographBlack = (Image)target;
			break;
		case 85:
			imgFavStylographBlue = (Image)target;
			break;
		case 86:
			imgFavStylographColorful = (Image)target;
			break;
		case 87:
			imgFavStylographGreen = (Image)target;
			break;
		case 88:
			imgFavStylographNoPen = (Image)target;
			break;
		case 89:
			imgFavStylographOrange = (Image)target;
			break;
		case 90:
			imgFavStylographRed = (Image)target;
			break;
		case 91:
			imgFavStylographWhite = (Image)target;
			break;
		case 92:
			imgFavTriangle = (Image)target;
			break;
		case 93:
			imgFavUndo = (Image)target;
			break;
		case 94:
			cnvBackgroundPaper = (Canvas)target;
			break;
		case 95:
			cnvCurtain = (Canvas)target;
			break;
		case 96:
			pathCurtain = (Path)target;
			break;
		case 97:
			rectOpaque = (RectangleGeometry)target;
			break;
		case 98:
			rectTransparent = (RectangleGeometry)target;
			break;
		case 99:
			cnvLibraryBack = (Canvas)target;
			break;
		case 100:
			gridGesture = (Grid)target;
			break;
		case 101:
			cnvGesture = (Canvas)target;
			break;
		case 102:
			gridExpandingCircle = (Grid)target;
			break;
		case 103:
			pathExpandingCircleDown = (Path)target;
			break;
		case 104:
			ellipseExpandingDown = (EllipseGeometry)target;
			break;
		case 105:
			pathExpandingCircleRight = (Path)target;
			break;
		case 106:
			ellipseExpandingRight = (EllipseGeometry)target;
			break;
		case 107:
			pathExpandingCircleUp = (Path)target;
			break;
		case 108:
			ellipseExpandingUp = (EllipseGeometry)target;
			break;
		case 109:
			pathExpandingCircleLeft = (Path)target;
			break;
		case 110:
			ellipseExpandingLeft = (EllipseGeometry)target;
			break;
		case 111:
			imgGestureLeft = (Image)target;
			break;
		case 112:
			imgGestureUp = (Image)target;
			break;
		case 113:
			imgGestureRight = (Image)target;
			break;
		case 114:
			imgGestureDown = (Image)target;
			break;
		case 115:
			imgGestureAnimation = (Image)target;
			break;
		case 116:
			inkCanvas = (InkCanvas)target;
			break;
		case 117:
			cnvLibraryFront = (Canvas)target;
			break;
		case 118:
			cnvLibrary = (Canvas)target;
			break;
		case 119:
			imgLibrary = (Image)target;
			break;
		case 120:
			rectLibrary = (Rectangle)target;
			break;
		case 121:
			imgResize = (Image)target;
			break;
		case 122:
			imgRotate = (Image)target;
			break;
		case 123:
			rectFrame = (Rectangle)target;
			break;
		case 124:
			cnvMainMenu = (Canvas)target;
			break;
		case 125:
			borderMainMenu = (Border)target;
			break;
		case 126:
			gridMainMenu = (Grid)target;
			break;
		case 127:
			imgTitleBar = (Image)target;
			break;
		case 128:
			imgHandAndPen = (Image)target;
			break;
		case 129:
			imgPen = (Image)target;
			break;
		case 130:
			imgTickOfPen = (Image)target;
			break;
		case 131:
			imgEraser = (Image)target;
			break;
		case 132:
			imgTickOfEraser = (Image)target;
			break;
		case 133:
			imgShape = (Image)target;
			break;
		case 134:
			imgTickOfShape = (Image)target;
			break;
		case 135:
			imgClose = (Image)target;
			break;
		case 136:
			imgSettings = (Image)target;
			break;
		case 137:
			GridSubMenuPenRight = (Grid)target;
			break;
		case 138:
			imgSubMenuPenRight = (Image)target;
			break;
		case 139:
			imgSize1SelectionR = (Image)target;
			break;
		case 140:
			imgSize2SelectionR = (Image)target;
			break;
		case 141:
			imgSize3SelectionR = (Image)target;
			break;
		case 142:
			imgSize4SelectionR = (Image)target;
			break;
		case 143:
			imgSize5SelectionR = (Image)target;
			break;
		case 144:
			imgSize6SelectionR = (Image)target;
			break;
		case 145:
			GridSubMenuPenLeft = (Grid)target;
			break;
		case 146:
			imgSubMenuPenLeft = (Image)target;
			break;
		case 147:
			imgSize1SelectionL = (Image)target;
			break;
		case 148:
			imgSize2SelectionL = (Image)target;
			break;
		case 149:
			imgSize3SelectionL = (Image)target;
			break;
		case 150:
			imgSize4SelectionL = (Image)target;
			break;
		case 151:
			imgSize5SelectionL = (Image)target;
			break;
		case 152:
			imgSize6SelectionL = (Image)target;
			break;
		case 153:
			GridSubMenuEraserRight = (Grid)target;
			break;
		case 154:
			imgSubMenuEraserRight = (Image)target;
			break;
		case 155:
			imgGrayedUndoRight = (Image)target;
			break;
		case 156:
			imgGrayedRedoRight = (Image)target;
			break;
		case 157:
			GridSubMenuEraserLeft = (Grid)target;
			break;
		case 158:
			imgSubMenuEraserLeft = (Image)target;
			break;
		case 159:
			imgGrayedUndoLeft = (Image)target;
			break;
		case 160:
			imgGrayedRedoLeft = (Image)target;
			break;
		case 161:
			GridSubMenuShapeRight = (Grid)target;
			break;
		case 162:
			imgSubMenuShapeRight = (Image)target;
			break;
		case 163:
			GridSubMenuShapeLeft = (Grid)target;
			break;
		case 164:
			imgSubMenuShapeLeft = (Image)target;
			break;
		case 165:
			GridSubMenuCloseRight = (Grid)target;
			break;
		case 166:
			imgSubMenuCloseRight = (Image)target;
			break;
		case 167:
			lblCloseRight = (Label)target;
			break;
		case 168:
			GridSubMenuCloseLeft = (Grid)target;
			break;
		case 169:
			imgSubMenuCloseLeft = (Image)target;
			break;
		case 170:
			lblCloseLeft = (Label)target;
			break;
		case 171:
			GridColorSelector = (Grid)target;
			break;
		case 172:
			imgColorSelector = (Image)target;
			break;
		case 173:
			borderSettings = (Border)target;
			break;
		case 174:
			imgCloseSettings = (Image)target;
			break;
		case 175:
			lblSettings = (Label)target;
			break;
		case 176:
			lblSettingsStartingPen = (Label)target;
			break;
		case 177:
			lblSettingsStartingPosition = (Label)target;
			break;
		case 178:
			lblSettingsFavourites = (Label)target;
			break;
		case 179:
			lblSettingsSpeedAccess = (Label)target;
			break;
		case 180:
			lblSettingsCloseConfirmation = (Label)target;
			break;
		case 181:
			lblSettingsAutoStart = (Label)target;
			break;
		case 182:
			lblAbout = (Label)target;
			break;
		case 183:
			GridLogo = (Grid)target;
			break;
		case 184:
			imgFatihPen = (Image)target;
			break;
		case 185:
			lblVersion = (Label)target;
			break;
		case 186:
			GridStartingPen = (Grid)target;
			break;
		case 187:
			lblPenType = (Label)target;
			break;
		case 188:
			lblInkSize = (Label)target;
			break;
		case 189:
			lblInkColor = (Label)target;
			break;
		case 190:
			ellipseInkColor = (Ellipse)target;
			break;
		case 191:
			lblSetCurrentPenState = (Label)target;
			break;
		case 192:
			lblSetDefaultPen = (Label)target;
			break;
		case 193:
			GridStartingPosition = (Grid)target;
			break;
		case 194:
			rectScreen = (Rectangle)target;
			break;
		case 195:
			imgMiniMainMenu = (Image)target;
			break;
		case 196:
			lblSetCurrentPosition = (Label)target;
			break;
		case 197:
			lblSetDefaultPosition = (Label)target;
			break;
		case 198:
			GridFavourites = (Grid)target;
			break;
		case 199:
			textBlockFovourites = (TextBlock)target;
			break;
		case 200:
			lblSaveFavourites = (Label)target;
			break;
		case 201:
			lblSetDefaultFavourites = (Label)target;
			break;
		case 202:
			GridSpeedAccess = (Grid)target;
			break;
		case 203:
			rectScreen2 = (Rectangle)target;
			break;
		case 204:
			imgsettingsGestureEllipse = (Image)target;
			break;
		case 205:
			imgsettingsGestureHand2Finger = (Image)target;
			break;
		case 206:
			chkGesture = (CheckBox)target;
			break;
		case 207:
			chkShortGesture = (CheckBox)target;
			break;
		case 208:
			chkSideArrows = (CheckBox)target;
			break;
		case 209:
			GridCloseConfirmation = (Grid)target;
			break;
		case 210:
			chkCloseConfirmation = (CheckBox)target;
			break;
		case 211:
			GridAutoStart = (Grid)target;
			break;
		case 212:
			chkAutoStart = (CheckBox)target;
			break;
		case 213:
			chkPreLoad = (CheckBox)target;
			break;
		case 214:
			textChkPreLoad = (AccessText)target;
			break;
		case 215:
			GridAbout = (Grid)target;
			break;
		case 216:
			textBlockAbout = (TextBlock)target;
			break;
		case 217:
			lblWebLink = (Label)target;
			break;
		case 218:
			borderSideArrowRight = (Border)target;
			break;
		case 219:
			imgSideArrowRight = (Image)target;
			break;
		case 220:
			borderSideArrowLeft = (Border)target;
			break;
		case 221:
			imgSideArrowLeft = (Image)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}

	void IComponentConnector.Connect(int connectionId, object target)
	{
		//ILSpy generated this explicit interface implementation from .override directive in System_Windows_Markup_IComponentConnector_Connect
		this.System_Windows_Markup_IComponentConnector_Connect(connectionId, target);
	}
}