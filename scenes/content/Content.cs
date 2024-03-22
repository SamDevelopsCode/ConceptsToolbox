using Godot;
using System;

public partial class Content : MarginContainer
{
	public override void _Ready()
	{
		if (Name == "DotNet")
		{
			GetNode<RichTextLabel>("%Title").Text = "[center][u].NET[/u][/center]";
		}
		else
		{
			GetNode<RichTextLabel>("%Title").Text = $"[center][u]{Name}[/u][/center]";
		}
	}
	
}
