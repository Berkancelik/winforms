﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;
using Xunit;
using Xunit.Abstractions;

namespace System.Windows.Forms.UITests;

public class DragDropTests : ControlTestBase
{
    public const int DragDropDelayMS = 100;
    public const string DragAcceptRtf = "DragAccept.rtf";
    public const string Resources = "Resources";
    private readonly Bitmap _dragImage = new("./Resources/move.bmp");

    public DragDropTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [WinFormsFact]
    public async Task DragDrop_QueryDefaultCursors_Async()
    {
        await RunFormWithoutControlAsync(() => new DragDropForm(TestOutputHelper), async (form) =>
        {
            await MoveMouseToControlAsync(form.ListDragSource);

            await InputSimulator.SendAsync(
                form,
                inputSimulator => inputSimulator.Mouse.LeftButtonDown());

            var targetMousePosition = ToVirtualPoint(form.ListDragTarget.PointToScreen(new Point(20, 20)));
            await InputSimulator.SendAsync(
                form,
                inputSimulator => inputSimulator.Mouse
                    .LeftButtonDown()
                    .Sleep(DragDropDelayMS)
                    .MoveMouseTo(targetMousePosition.X - 40, targetMousePosition.Y)
                    .Sleep(DragDropDelayMS)
                    .MoveMouseTo(targetMousePosition.X, targetMousePosition.Y)
                    .Sleep(DragDropDelayMS) // slight delay so drag&drop triggered
                    .MoveMouseTo(targetMousePosition.X + 2, targetMousePosition.Y + 2)
                    .Sleep(DragDropDelayMS) // slight delay so drag&drop triggered
                    .MoveMouseTo(targetMousePosition.X + 4, targetMousePosition.Y + 4)
                    .Sleep(DragDropDelayMS)
                    .LeftButtonUp()
                    .Sleep(DragDropDelayMS));

            Assert.Equal(1, form.ListDragTarget.Items.Count);
        });
    }

    [WinFormsFact]
    public async Task DragEnter_Set_DropImageType_Message_MessageReplacementToken_ReturnsExptected_Async()
    {
        await RunFormWithoutControlAsync(() => new DragDropForm(TestOutputHelper), async (form) =>
        {
            DropImageType dropImageType = DropImageType.Move;
            string message = "Move to %1";
            string messageReplacementToken = "Drop Target";

            form.ListDragSource.GiveFeedback += (s, e) =>
            {
                e.DragImage = _dragImage;
                e.CursorOffset = new Point(0, 48);
                e.UseDefaultCursors = false;
            };

            form.ListDragTarget.DragEnter += (s, e) =>
            {
                e.DropImageType = dropImageType;
                e.Message = message;
                e.MessageReplacementToken = messageReplacementToken;
                e.Effect = DragDropEffects.Copy;
            };

            form.ListDragTarget.DragOver += (s, e) =>
            {
                Assert.Equal(dropImageType, e.DropImageType);
                Assert.Equal(message, e.Message);
                Assert.Equal(messageReplacementToken, e.MessageReplacementToken);
            };

            form.ListDragTarget.DragDrop += (s, e) =>
            {
                Assert.Equal(dropImageType, e.DropImageType);
                Assert.Equal(message, e.Message);
                Assert.Equal(messageReplacementToken, e.MessageReplacementToken);
            };

            var startRect = form.ListDragSource.DisplayRectangle;
            var centerOfStartRect = new Point(startRect.Left, startRect.Top) + new Size(startRect.Width / 2, startRect.Height / 2);
            var startCoordinates = form.ListDragSource.PointToScreen(centerOfStartRect);
            var endRect = form.ListDragTarget.DisplayRectangle;
            var centerOfEndtRect = new Point(endRect.Left, startRect.Top) + new Size(endRect.Width / 2, endRect.Height / 2);
            var endCoordinates = form.ListDragTarget.PointToScreen(centerOfEndtRect);
            var virtualPointStart = ToVirtualPoint(startCoordinates);
            var virtualPointEnd = ToVirtualPoint(endCoordinates);

            await InputSimulator.SendAsync(
                form,
                inputSimulator
                    => inputSimulator
                        .Mouse.MoveMouseTo(virtualPointStart.X + 6, virtualPointStart.Y + 6)
                        .LeftButtonDown()
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X, virtualPointEnd.Y)
                        // The drag and drop is very finicky, and if we just call LeftButtonUp()
                        // it won't work... It'd for some reason think we'd left the control instead.
                        //
                        // To work around it - give it a full second to react and then
                        // simulate a mouse click.
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X, virtualPointEnd.Y)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 2, virtualPointEnd.Y + 2)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 4, virtualPointEnd.Y + 4)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 2, virtualPointEnd.Y + 2)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 4, virtualPointEnd.Y + 4)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 2, virtualPointEnd.Y + 2)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 4, virtualPointEnd.Y + 4)
                        .Sleep(DragDropDelayMS)
                        .LeftButtonClick()
                        .Sleep(DragDropDelayMS));

            await InputSimulator.SendAsync(
                form,
                inputSimulator
                    => inputSimulator
                        .Mouse.MoveMouseTo(virtualPointStart.X + 6, virtualPointStart.Y + 6)
                        .LeftButtonDown()
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X, virtualPointEnd.Y)
                        // The drag and drop is very finicky, and if we just call LeftButtonUp()
                        // it won't work... It'd for some reason think we'd left the control instead.
                        //
                        // To work around it - give it a full second to react and then
                        // simulate a mouse click.
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X, virtualPointEnd.Y)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 2, virtualPointEnd.Y + 2)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 4, virtualPointEnd.Y + 4)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 2, virtualPointEnd.Y + 2)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 4, virtualPointEnd.Y + 4)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 2, virtualPointEnd.Y + 2)
                        .Sleep(DragDropDelayMS)
                        .MoveMouseTo(virtualPointEnd.X + 4, virtualPointEnd.Y + 4)
                        .Sleep(DragDropDelayMS)
                        .LeftButtonClick()
                        .Sleep(DragDropDelayMS));

            Assert.Equal(2, form.ListDragTarget.Items.Count);
        });
    }

    [WinFormsFact]
    public async Task PictureBox_SetData_DoDragDrop_RichTextBox_ReturnsExptected_Async()
    {
        await RunFormWithoutControlAsync(() => new DragImageDropDescriptionForm(TestOutputHelper), async (form) =>
        {
            string dragAcceptRtfPath = Path.Combine(Directory.GetCurrentDirectory(), Resources, DragAcceptRtf);
            using RichTextBox richTextBox = new();
            richTextBox.Rtf = File.ReadAllText(dragAcceptRtfPath);
            string dragAcceptRtfContent = richTextBox.Rtf;
            string dragAcceptRtfTextContent = richTextBox.Text;
            Point startCoordinates = form.PictureBoxDragSource.PointToScreen(new Point(20, 20));
            Point virtualPointStart = ToVirtualPoint(startCoordinates);
            startCoordinates.Offset(155, 0);
            Point virtualPointEnd = ToVirtualPoint(startCoordinates);
            await InputSimulator.SendAsync(
                        form,
                        inputSimulator => inputSimulator.Mouse.MoveMouseTo(virtualPointStart.X, virtualPointStart.Y)
                                                                .LeftButtonDown()
                                                                .Sleep(DragDropDelayMS)
                                                                .MoveMouseTo(virtualPointEnd.X, virtualPointEnd.Y)
                                                                .Sleep(DragDropDelayMS)
                                                                .LeftButtonUp()
                                                                .Sleep(DragDropDelayMS));

            Assert.NotNull(form);
            Assert.NotNull(form.RichTextBoxDropTarget);
            Assert.False(string.IsNullOrWhiteSpace(form.RichTextBoxDropTarget.Rtf));
            Assert.False(string.IsNullOrWhiteSpace(form.RichTextBoxDropTarget.Text));
            Assert.Equal(dragAcceptRtfContent, form.RichTextBoxDropTarget?.Rtf);
            Assert.Equal(dragAcceptRtfTextContent, form.RichTextBoxDropTarget?.Text);
        });
    }

    [WinFormsFact]
    public async Task ToolStripItem_SetData_DoDragDrop_RichTextBox_ReturnsExptected_Async()
    {
        await RunFormWithoutControlAsync(() => new DragImageDropDescriptionForm(TestOutputHelper), async (form) =>
        {
            string dragAcceptRtfPath = Path.Combine(Directory.GetCurrentDirectory(), Resources, DragAcceptRtf);
            using RichTextBox richTextBox = new();
            richTextBox.Rtf = File.ReadAllText(dragAcceptRtfPath);
            string dragAcceptRtfContent = richTextBox.Rtf;
            string dragAcceptRtfTextContent = richTextBox.Text;

            await MoveMouseToControlAsync(form.ToolStrip);
            await InputSimulator.SendAsync(
                form,
                inputSimulator => inputSimulator.Mouse.LeftButtonClick()
                                                      .Sleep(DragDropDelayMS));

            Point toolStripItemCoordinates = form.ToolStrip.PointToScreen(new Point(5, 5));
            toolStripItemCoordinates.Offset(0, 40);
            Point virtualToolStripItemCoordinates = ToVirtualPoint(toolStripItemCoordinates);

            await InputSimulator.SendAsync(
                        form,
                        inputSimulator => inputSimulator.Mouse.MoveMouseTo(virtualToolStripItemCoordinates.X, virtualToolStripItemCoordinates.Y));

            Point virtualPointStart = virtualToolStripItemCoordinates;
            toolStripItemCoordinates.Offset(50, 50);
            Point virtualPointEnd = ToVirtualPoint(toolStripItemCoordinates);
            await InputSimulator.SendAsync(
                        form,
                        inputSimulator => inputSimulator.Mouse.MoveMouseTo(virtualPointStart.X, virtualPointStart.Y)
                                                                .LeftButtonDown()
                                                                .Sleep(DragDropDelayMS)
                                                                .MoveMouseTo(virtualPointEnd.X, virtualPointEnd.Y)
                                                                .Sleep(DragDropDelayMS)
                                                                .LeftButtonUp()
                                                                .Sleep(DragDropDelayMS));

            Assert.NotNull(form);
            Assert.NotNull(form.RichTextBoxDropTarget);
            Assert.False(string.IsNullOrWhiteSpace(form.RichTextBoxDropTarget.Rtf));
            Assert.False(string.IsNullOrWhiteSpace(form.RichTextBoxDropTarget.Text));
            Assert.Equal(dragAcceptRtfContent, form.RichTextBoxDropTarget?.Rtf);
            Assert.Equal(dragAcceptRtfTextContent, form.RichTextBoxDropTarget?.Text);
        });
    }

    class DragDropForm : Form
    {
        public ListBox ListDragSource;
        public ListBox ListDragTarget;
        private CheckBox UseCustomCursorsCheck;
        private Label DropLocationLabel;

        private int indexOfItemUnderMouseToDrag;
        private int indexOfItemUnderMouseToDrop;

        private Rectangle dragBoxFromMouseDown;
        private Point screenOffset;

        private Cursor? MyNoDropCursor;
        private Cursor? MyNormalCursor;

        private readonly ITestOutputHelper _testOutputHelper;

        public DragDropForm(ITestOutputHelper testOutputHelper)
        {
            ListDragSource = new ListBox();
            ListDragTarget = new ListBox();
            UseCustomCursorsCheck = new CheckBox();
            DropLocationLabel = new Label();

            SuspendLayout();

            // ListDragSource
            ListDragSource.Items.AddRange(new object[]
            {
                "one", "two", "three", "four",
                "five", "six", "seven", "eight",
                "nine", "ten"
            });
            ListDragSource.Location = new Point(10, 17);
            ListDragSource.Size = new Size(120, 225);
            ListDragSource.MouseDown += ListDragSource_MouseDown;
            ListDragSource.QueryContinueDrag += ListDragSource_QueryContinueDrag;
            ListDragSource.MouseUp += ListDragSource_MouseUp;
            ListDragSource.MouseMove += ListDragSource_MouseMove;
            ListDragSource.GiveFeedback += ListDragSource_GiveFeedback;

            // ListDragTarget
            ListDragTarget.AllowDrop = true;
            ListDragTarget.Location = new Point(154, 17);
            ListDragTarget.Size = new Size(120, 225);
            ListDragTarget.DragOver += ListDragTarget_DragOver;
            ListDragTarget.DragDrop += ListDragTarget_DragDrop;
            ListDragTarget.DragEnter += ListDragTarget_DragEnter;
            ListDragTarget.DragLeave += ListDragTarget_DragLeave;

            // UseCustomCursorsCheck
            UseCustomCursorsCheck.Location = new Point(10, 243);
            UseCustomCursorsCheck.Size = new Size(137, 24);
            UseCustomCursorsCheck.Text = "Use Custom Cursors";

            // DropLocationLabel
            DropLocationLabel.Location = new Point(154, 245);
            DropLocationLabel.Size = new Size(137, 24);
            DropLocationLabel.Text = "None";

            // Form1
            ClientSize = new Size(292, 270);
            Controls.AddRange(new Control[]
            {
                ListDragSource,
                ListDragTarget,
                UseCustomCursorsCheck,
                DropLocationLabel
            });

            _testOutputHelper = testOutputHelper;
        }

        private void ListDragSource_MouseDown(object? sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            indexOfItemUnderMouseToDrag = ListDragSource.IndexFromPoint(e.X, e.Y);
            _testOutputHelper.WriteLine($"Mouse down on drag source at position ({e.X},{e.Y}). Index of element under mouse: {indexOfItemUnderMouseToDrag}");

            if (indexOfItemUnderMouseToDrag != ListBox.NoMatches)
            {
                // Remember the point where the mouse down occurred. The DragSize indicates
                // the size that the mouse can move before a drag event should be started.
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(
                    new Point(e.X - (dragSize.Width / 2),
                              e.Y - (dragSize.Height / 2)),
                    dragSize);
            }
            else
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
            }
        }

        private void ListDragSource_MouseUp(object? sender, MouseEventArgs e)
        {
            // Reset the drag rectangle when the mouse button is raised.
            dragBoxFromMouseDown = Rectangle.Empty;
            _testOutputHelper.WriteLine($"Mouse up on drag source at position ({e.X},{e.Y}).");
        }

        private void ListDragSource_MouseMove(object? sender, MouseEventArgs e)
        {
            _testOutputHelper.WriteLine($"Mouse move on drag source to position ({e.X},{e.Y}) with buttons {e.Button}.");
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Create custom cursors for the drag-and-drop operation.
                    try
                    {
                        MyNormalCursor = new Cursor("./Resources/3dwarro.cur");
                        MyNoDropCursor = new Cursor("./Resources/3dwno.cur");
                    }
                    catch
                    {
                        // An error occurred while attempting to load the cursors, so use
                        // standard cursors.
                        UseCustomCursorsCheck.Checked = false;
                    }
                    finally
                    {
                        // The screenOffset is used to account for any desktop bands
                        // that may be at the top or left side of the screen when
                        // determining when to cancel the drag drop operation.
                        screenOffset = SystemInformation.WorkingArea.Location;

                        // Proceed with the drag-and-drop, passing in the list item.
                        DragDropEffects dropEffect = ListDragSource.DoDragDrop(
                            ListDragSource.Items[indexOfItemUnderMouseToDrag],
                            DragDropEffects.All | DragDropEffects.Link);

                        // If the drag operation was a move then remove the item.
                        if (dropEffect == DragDropEffects.Move)
                        {
                            ListDragSource.Items.RemoveAt(indexOfItemUnderMouseToDrag);

                            // Selects the previous item in the list as long as the list has an item.
                            if (indexOfItemUnderMouseToDrag > 0)
                                ListDragSource.SelectedIndex = indexOfItemUnderMouseToDrag - 1;

                            else if (ListDragSource.Items.Count > 0)
                                // Selects the first item.
                                ListDragSource.SelectedIndex = 0;
                        }

                        // Dispose of the cursors since they are no longer needed.
                        if (MyNormalCursor != null)
                            MyNormalCursor.Dispose();

                        if (MyNoDropCursor != null)
                            MyNoDropCursor.Dispose();
                    }
                }
            }
        }

        private void ListDragSource_GiveFeedback(object? sender, GiveFeedbackEventArgs e)
        {
            _testOutputHelper.WriteLine($"Give feedback on drag source.");

            // Use custom cursors if the check box is checked.
            if (UseCustomCursorsCheck.Checked)
            {
                // Sets the custom cursor based upon the effect.
                e.UseDefaultCursors = false;
                if ((e.Effect & DragDropEffects.Move) == DragDropEffects.Move)
                    Cursor.Current = MyNormalCursor;
                else
                    Cursor.Current = MyNoDropCursor;
            }
        }

        private void ListDragTarget_DragOver(object? sender, DragEventArgs e)
        {
            _testOutputHelper.WriteLine($"Drag over on the drag target.");

            // Determine whether string data exists in the drop data. If not, then
            // the drop effect reflects that the drop cannot occur.
            if (e.Data is null || !e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.None;
                DropLocationLabel.Text = "None - no string data.";
                return;
            }

            // Set the effect based upon the KeyState.
            if ((e.KeyState & (8 + 32)) == (8 + 32) &&
                (e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link)
            {
                // KeyState 8 + 32 = CTRL + ALT

                // Link drag-and-drop effect.
                e.Effect = DragDropEffects.Link;
            }
            else if ((e.KeyState & 32) == 32 &&
                (e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link)
            {
                // ALT KeyState for link.
                e.Effect = DragDropEffects.Link;
            }
            else if ((e.KeyState & 4) == 4 &&
                (e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {
                // SHIFT KeyState for move.
                e.Effect = DragDropEffects.Move;
            }
            else if ((e.KeyState & 8) == 8 &&
                (e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                // CTRL KeyState for copy.
                e.Effect = DragDropEffects.Copy;
            }
            else if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {
                // By default, the drop action should be move, if allowed.
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

            // Get the index of the item the mouse is below.

            // The mouse locations are relative to the screen, so they must be
            // converted to client coordinates.

            indexOfItemUnderMouseToDrop =
                ListDragTarget.IndexFromPoint(ListDragTarget.PointToClient(new Point(e.X, e.Y)));

            // Updates the label text.
            if (indexOfItemUnderMouseToDrop != ListBox.NoMatches)
            {
                DropLocationLabel.Text = "Drops before item #" + (indexOfItemUnderMouseToDrop + 1);
            }
            else
            {
                DropLocationLabel.Text = "Drops at the end.";
            }
        }

        private void ListDragTarget_DragDrop(object? sender, DragEventArgs e)
        {
            _testOutputHelper.WriteLine($"Drag drop on drag target.");

            // Ensure that the list item index is contained in the data.
            if (e.Data is not null && e.Data.GetDataPresent(typeof(string)))
            {
                object? item = e.Data.GetData(typeof(string));

                // Perform drag-and-drop, depending upon the effect.
                if (item is not null && (e.Effect == DragDropEffects.Copy || e.Effect == DragDropEffects.Move))
                {
                    // Insert the item.
                    if (indexOfItemUnderMouseToDrop != ListBox.NoMatches)
                        ListDragTarget.Items.Insert(indexOfItemUnderMouseToDrop, item);
                    else
                        ListDragTarget.Items.Add(item);
                }
            }

            // Reset the label text.
            DropLocationLabel.Text = "None";
        }

        private void ListDragSource_QueryContinueDrag(object? sender, QueryContinueDragEventArgs e)
        {
            _testOutputHelper.WriteLine($"Query for drag continuation.");

            // Cancel the drag if the mouse moves off the form.
            if (sender is ListBox lb)
            {
                Form form = lb.FindForm()!;

                // Cancel the drag if the mouse moves off the form. The screenOffset
                // takes into account any desktop bands that may be at the top or left
                // side of the screen.
                if (((MousePosition.X - screenOffset.X) < form.DesktopBounds.Left) ||
                    ((MousePosition.X - screenOffset.X) > form.DesktopBounds.Right) ||
                    ((MousePosition.Y - screenOffset.Y) < form.DesktopBounds.Top) ||
                    ((MousePosition.Y - screenOffset.Y) > form.DesktopBounds.Bottom))
                {
                    _testOutputHelper.WriteLine($"Cancelling drag.");
                    e.Action = DragAction.Cancel;
                }
            }
        }

        private void ListDragTarget_DragEnter(object? sender, DragEventArgs e)
        {
            _testOutputHelper.WriteLine($"Drag enter on target.");

            // Reset the label text.
            DropLocationLabel.Text = "None";
        }

        private void ListDragTarget_DragLeave(object? sender, EventArgs e)
        {
            _testOutputHelper.WriteLine($"Drag leave on target.");

            // Reset the label text.
            DropLocationLabel.Text = "None";
        }
    }

    class DragImageDropDescriptionForm : Form
    {
        private readonly Bitmap _dragImage = new("./Resources/image.png");
        private readonly Bitmap _dragAcceptBmp = new("./Resources/DragAccept.bmp");
        private readonly ITestOutputHelper _testOutputHelper;

        private ContextMenuStrip? _contextMenuStrip;

        public PictureBox PictureBoxDragSource;
        public RichTextBox RichTextBoxDropTarget;
        public ToolStrip ToolStrip = new();

        public DragImageDropDescriptionForm(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            PictureBoxDragSource = new PictureBox();
            RichTextBoxDropTarget = new RichTextBox();

            SuspendLayout();

            // PictureBoxDragSource
            PictureBoxDragSource.AllowDrop = true;
            PictureBoxDragSource.BorderStyle = BorderStyle.FixedSingle;
            PictureBoxDragSource.Location = new Point(10, 45);
            PictureBoxDragSource.Size = new Size(125, 119);
            PictureBoxDragSource.DragEnter += PictureBoxDragSource_DragEnter;
            PictureBoxDragSource.DragOver += PictureBoxDragSource_DragOver;
            PictureBoxDragSource.MouseDown += PictureBoxDragSource_MouseDown;

            // RichTextBoxDropTarget
            RichTextBoxDropTarget.AllowDrop = true;
            RichTextBoxDropTarget.EnableAutoDragDrop = true;
            RichTextBoxDropTarget.Location = new Point(145, 45);
            RichTextBoxDropTarget.Size = new Size(125, 119);
            RichTextBoxDropTarget.DragEnter += RichTextBoxDropTarget_DragEnter;
            RichTextBoxDropTarget.DragDrop += RichTextBoxDropTarget_DragDrop;

            // ToolStrip
            CreateToolStrip();

            // Form1
            ClientSize = new Size(285, 175);
            Controls.AddRange(new Control[]
            {
                PictureBoxDragSource,
                RichTextBoxDropTarget
            });
        }

        private void CreateToolStrip()
        {
            TableLayoutPanel tableLayoutPanel = new()
            {
                ColumnCount = 1,
                Dock = DockStyle.Top,
                Height = 35,
                RowCount = 1
            };

            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _contextMenuStrip = new ContextMenuStrip
            {
                AllowDrop = true,
                AutoSize = true,
                ImageScalingSize = new Size(25, 25)
            };

            _contextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ToolStrip = new()
            {
                DefaultDropDownDirection = ToolStripDropDownDirection.BelowLeft,
                Dock = DockStyle.Right,
                GripStyle = ToolStripGripStyle.Hidden
            };

            ToolStripDropDownButton toolStripDropDownButton = new()
            {
                AutoSize = false,
                AutoToolTip = false,
                DropDown = _contextMenuStrip,
                Height = 35,
                Name = "toolStripDropDownButton",
                Text = "Drag Images",
                ToolTipText = string.Empty,
                Width = 100
            };

            ToolStrip.Items.Add(toolStripDropDownButton);
            tableLayoutPanel.Controls.Add(ToolStrip, 0, 0);
            Controls.Add(tableLayoutPanel);
            ContextMenuStrip = _contextMenuStrip;
        }

        void ContextMenuStrip_Opening(object? sender, CancelEventArgs e)
        {
            if (_contextMenuStrip is null)
            {
                return;
            }

            _contextMenuStrip.DefaultDropDownDirection = ToolStripDropDownDirection.BelowLeft;
            _contextMenuStrip.Items.Clear();

            ToolStripItem dragAcceptItem = new ToolStripMenuItem()
            {
                AllowDrop = true,
                Image = _dragAcceptBmp,
                ImageScaling = ToolStripItemImageScaling.SizeToFit,
                Text = "DragAccept",
                Name = "dragAcceptItem",
            };
            dragAcceptItem.DragEnter += DragAcceptItem_DragEnter;
            dragAcceptItem.MouseDown += DragAcceptItem_MouseDown;

            _contextMenuStrip.Items.Add(dragAcceptItem);
            e.Cancel = false;
        }

        private void DragAcceptItem_DragEnter(object? sender, DragEventArgs e)
        {
            _testOutputHelper.WriteLine($"Drag enter on target.");

            e.DropImageType = DropImageType.Link;
            e.Message = "DragAcceptFiles";
            e.Effect = DragDropEffects.Link;
        }

        private void DragAcceptItem_MouseDown(object? sender, MouseEventArgs e)
        {
            _testOutputHelper.WriteLine($"Mouse down on drag source at position ({e.X},{e.Y}).");

            if (sender is not ToolStripItem dragAcceptItem)
            {
                return;
            }

            string dragAcceptRtf = Path.Combine(Directory.GetCurrentDirectory(), Resources, DragAcceptRtf);
            if (File.Exists(dragAcceptRtf))
            {
                string[] dropFiles = new string[] { dragAcceptRtf };
                DataObject data = new(DataFormats.FileDrop, dropFiles);
                dragAcceptItem.DoDragDrop(data, DragDropEffects.All, _dragAcceptBmp, new Point(0, 16), false);
            }
        }

        private void PictureBoxDragSource_DragOver(object? sender, DragEventArgs e)
        {
            _testOutputHelper.WriteLine($"Drag over on source.");

            e.DropImageType = DropImageType.None;
            e.Effect = DragDropEffects.None;
        }

        private void RichTextBoxDropTarget_DragDrop(object? sender, DragEventArgs e)
        {
            _testOutputHelper.WriteLine($"Drag drop on target.");

            if (e.Data is not null
                && e.Data.GetDataPresent(DataFormats.FileDrop)
                && e.Data.GetData(DataFormats.FileDrop) is string[] fileNames
                && fileNames.Length > 0 && fileNames[0].Contains(DragAcceptRtf))
            {
                RichTextBoxDropTarget.Clear();
                RichTextBoxDropTarget.LoadFile(fileNames[0], RichTextBoxStreamType.RichText);
                e.Effect = DragDropEffects.None;
            }
        }

        private void RichTextBoxDropTarget_DragEnter(object? sender, DragEventArgs e)
        {
            _testOutputHelper.WriteLine($"Drag enter on target.");

            if (e.Data is not null
                && e.Data.GetDataPresent(DataFormats.FileDrop)
                && e.Data.GetData(DataFormats.FileDrop) is string[] fileNames
                && fileNames.Length > 0 && fileNames[0].Contains(DragAcceptRtf))
            {
                e.DropImageType = DropImageType.Link;
                e.Message = "%1 (shellapi.h)";
                e.MessageReplacementToken = "DragAcceptFiles";
                e.Effect = DragDropEffects.Link;
            }
        }

        private void PictureBoxDragSource_DragEnter(object? sender, DragEventArgs e)
        {
            _testOutputHelper.WriteLine($"Drag enter on source.");

            e.DropImageType = DropImageType.None;
            e.Effect = DragDropEffects.None;
        }

        private void PictureBoxDragSource_MouseDown(object? sender, MouseEventArgs e)
        {
            _testOutputHelper.WriteLine($"Mouse down on drag source at position ({e.X},{e.Y}).");

            string dragAcceptRtf = Path.Combine(Directory.GetCurrentDirectory(), Resources, DragAcceptRtf);
            if (File.Exists(dragAcceptRtf))
            {
                string[] dropFiles = new string[] { dragAcceptRtf };
                DataObject data = new(DataFormats.FileDrop, dropFiles);
                PictureBoxDragSource.DoDragDrop(data, DragDropEffects.All, _dragImage, new Point(0, 16), false);
            }
        }
    }
}
