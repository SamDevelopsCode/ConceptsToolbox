using System.IO;
using Godot;
using System.Threading.Tasks;

public partial class ConceptSceneLoader : VBoxContainer
{
	[ExportCategory("Main Content Container")]
	[Export] private VBoxContainer _contentContainer;

	[Export] private PackedScene _contentScene;
	[Export] private PackedScene _textComponentScene;
	[Export] private PackedScene _codeComponentScene;
	
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
			var currentConcept = _contentContainer.GetChild(0);

			if (currentConcept.Name == button.Text)
			{
				return;
			}
			
			await TweenConceptContentInstanceExit(currentConcept);
		}

		AddConceptContentPage();
	}

	
	private async void AddConceptContentPage()
	{
		ContentPage contentPage = _contentScene.Instantiate<ContentPage>();
		GD.Print(contentPage.Vbox);
		_contentContainer.AddChild(contentPage);

		// Get the data from file
		if (File.Exists("C:\\Users\\there\\OneDrive\\Desktop\\variables.txt"))
		{
			string contentText = File.ReadAllText("C:\\Users\\there\\OneDrive\\Desktop\\variables.txt");
			GD.Print(contentText);
		}
		
		
		TextComponent newTextComponent = _textComponentScene.Instantiate<TextComponent>();
		newTextComponent.RichTextLabel.Text = "hello";
		contentPage.Vbox.AddChild(newTextComponent);

		RichTextLabel newCodeComponent = _codeComponentScene.Instantiate<RichTextLabel>();
		newCodeComponent.Text = "I am code here;";
		contentPage.Vbox.AddChild(newCodeComponent);
		
		
		// using modulation instead of hiding/showing because of weird flickering on load
		contentPage.Modulate = new Color(0, 0, 0, 0);
		
		// docs say to await a process frame before setting scale of a control that is a child of a container on load
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		
		contentPage.Modulate = Colors.White;
		contentPage.Scale = new Vector2(.01f, .01f);
		contentPage.PivotOffset = contentPage.Size / 2;
		
		TweenConceptContentInstanceEntry(contentPage);
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