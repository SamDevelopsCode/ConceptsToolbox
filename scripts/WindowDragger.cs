using Godot;
using System;

public partial class WindowDragger : Control
{
	private bool _dragging;
	private Vector2 _startPosition;

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			_dragging = !_dragging;
			_startPosition = GetLocalMousePosition();
		}
		
		if (_dragging)
		{
			DisplayServer.WindowSetPosition((DisplayServer.WindowGetPosition() + (Vector2I)(GetLocalMousePosition() - _startPosition)));
		}
		
		if (@event is InputEventMouseButton mouseEvent2 && mouseEvent2.DoubleClick)
		{
			if (DisplayServer.WindowGetMode(0) == DisplayServer.WindowMode.ExclusiveFullscreen)
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			}
			else
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
			}
		}
	}
}
