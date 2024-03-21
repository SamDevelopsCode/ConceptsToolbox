using Godot;
using System.Threading.Tasks;

public partial class ConceptSceneLoader : VBoxContainer
{
	[ExportCategory("Main Content Container")]
	[Export] private VBoxContainer _contentContainer;

	private bool _transitioningContentScenes;

	public override void _Ready()
	{
		foreach (ConceptButton childButton in GetChildren())
		{
			// Manually setting .NET button text in editor because you aren't allowed to use '.' in Node names,
			// this check is for every other concept page
			if (childButton.Text == "")
			{
				childButton.Text = childButton.Name;
			}
			
			childButton.TooltipText = childButton.Text;
			childButton.Pressed += () => OnButtonPressed(childButton);
		}
	}

	
	private async void OnButtonPressed(ConceptButton button)
	{
		if (_transitioningContentScenes) return;
		
		if (_contentContainer.GetChildCount() > 0)
		{
			var currentConceptContentInstance = _contentContainer.GetChild(0);

			if (currentConceptContentInstance.Name == button.Text)
			{
				return;
			}
			
			await TweenConceptContentInstanceExit(currentConceptContentInstance);
		}

		AddConceptContentPage(button.conceptScene, button.Text);
	}

	
	private async void AddConceptContentPage(PackedScene packedScene, string buttonText)
	{
		Control newConceptContentInstance = packedScene.Instantiate<Control>();
		_contentContainer.AddChild(newConceptContentInstance);
		
		// using modulation in method instead of hiding/showing because of weird flickering on load
		newConceptContentInstance.Modulate = new Color(0, 0, 0, 0);
		newConceptContentInstance.GetNode<Label>("%Title").Text = buttonText;
		
		
		// docs say to await a process frame before setting scale of a control that is a child of a container on load
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		
		newConceptContentInstance.Modulate = Colors.White;
		newConceptContentInstance.Scale = new Vector2(.01f, .01f);
		newConceptContentInstance.PivotOffset = newConceptContentInstance.Size / 2;
		
		TweenConceptContentInstanceEntry(newConceptContentInstance);
	}


	private void TweenConceptContentInstanceEntry(Control conceptContentInstance)
	{
		Tween tween = GetTree().CreateTween();
		tween.SetParallel();
		tween.TweenProperty(conceptContentInstance, "scale:x", 1.0f, .3f);
		tween.TweenProperty(conceptContentInstance, "scale:y", 1.0f, .3f);
	}
	
	
	private async Task TweenConceptContentInstanceExit(Node currentConceptContentInstance)
	{
		_transitioningContentScenes = true;
		Tween tween = GetTree().CreateTween();
		tween.SetParallel();
		tween.TweenProperty(currentConceptContentInstance, "scale:x", 0.01f, .3f);
		tween.TweenProperty(currentConceptContentInstance, "scale:y", 0.01f, .3f);
		await ToSignal(tween, Tween.SignalName.Finished);
		
		currentConceptContentInstance.QueueFree();
		
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		_transitioningContentScenes = false;
	}
}