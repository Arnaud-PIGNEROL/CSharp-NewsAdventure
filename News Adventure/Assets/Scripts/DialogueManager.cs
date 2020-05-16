using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public string NewMap;
    public Animator panelAnimator; 
    private Queue<string> sentences = new Queue<string>();
    private Queue<string> transition = new Queue<string>();

    public void StartDialogue(Dialogue dialogue)
    {

        nameText.text = dialogue.name;

        foreach (string sentence in dialogue.sentences)
        {
            this.sentences.Enqueue(sentence);
        }
        foreach (string transition in dialogue.changement)
        {
            this.transition.Enqueue(transition);
        }
            DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (this.sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        string nameTransition = transition.Dequeue();
        if (nameTransition.CompareTo("false") == 1)
            panelAnimator.SetBool(nameTransition, true);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        SceneManager.LoadScene(NewMap);
    }

}
