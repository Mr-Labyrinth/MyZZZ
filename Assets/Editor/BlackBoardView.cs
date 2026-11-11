using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class BlackBoardView : VisualElement
{
    Editor editor;

    public BlackBoardView()
    {

    }

    internal void UpdateSelection(BlackBoard blackBoard)
    {
        Clear();

        Object.DestroyImmediate(editor);

        editor = Editor.CreateEditor(blackBoard);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if (editor)
            {
                editor.OnInspectorGUI();
            }
        });

        Add(container);
    }
}
