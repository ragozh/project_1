using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogText;
    private Queue<string> sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialog(Dialog dialogue)
    {
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence(string text = "")
    {
        string sentence = "";
        if(string.IsNullOrEmpty(text))
        {
            if(sentences.Count == 0)
            {
                EndDialog();
                return;
            }
            sentence = sentences.Dequeue();
        } else {
            sentence = text;
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    
    IEnumerator TypeSentence (string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }

    public void EndDialog()
    {
        Debug.Log("End dialog");
    }
}
