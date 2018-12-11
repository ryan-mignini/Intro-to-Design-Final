using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditsScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI creditText;
    [SerializeField]
    private Camera cam;

    private Color desiredBackgroundColor;
    private Transform creditTransform;
    private Vector3 textVel;
    private float colorLerpSpeed = 1f;
	// Use this for initialization
	void Start ()
    {
        desiredBackgroundColor = cam.backgroundColor;
        creditTransform = creditText.gameObject.transform;
        textVel = Vector3.zero;

        StartCoroutine(DoCredits());
	}
	
	void FixedUpdate ()
    {
        cam.backgroundColor = Color.Lerp(cam.backgroundColor, desiredBackgroundColor, colorLerpSpeed*Time.fixedDeltaTime);
        creditTransform.position += textVel * Time.fixedDeltaTime;
	}

    private IEnumerator DoCredits()
    {
        //I apologize in advance - there's going to be a lot of magic numbers and other terrible coding practices in this function.

        string fullCreditText = creditText.text;
        int charactersRevealed = 0;

        creditText.text = "";
        yield return new WaitForSeconds(1);


        for (int i = 0; i < 11; i++)
        {
            //Initializing variables
            int totalCharactersToReveal = 0;
            float textSpeed = 1/30f;
            int readoutType = 0;
            float pauseAfterSection = 1f;
            int charactersToFade = 4;

            //Setting variables on a per-section basis
            switch (i)
            {
                case 0:  //Stardate
                    totalCharactersToReveal = 42;
                    break;
                case 1: //Title drop
                    totalCharactersToReveal = 121;
                    break;
                case 2:  //Epilogue header
                    totalCharactersToReveal = 139;
                    textSpeed = 1 / 5f;
                    break;
                case 3:  //Plot recap
                    totalCharactersToReveal = 310;
                    readoutType = 1;
                    pauseAfterSection = 0.25f;
                    break;
                case 4:  //Sequel (Continuance?) hook
                    totalCharactersToReveal = 344;
                    pauseAfterSection = 7.5f;
                    break;
                case 5: //Credits: Repeating the title
                    totalCharactersToReveal = 482;
                    pauseAfterSection = .25f;
                    readoutType = 2;
                    break;
                case 6: //Credits: Seth
                    totalCharactersToReveal = 571;
                    pauseAfterSection = .5f;
                    readoutType = 2;
                    break;
                case 7: //Credits: Adam
                    totalCharactersToReveal = 664;
                    pauseAfterSection = .75f;
                    readoutType = 2;
                    break;
                case 8: //Credits: Catherine
                    totalCharactersToReveal = 758;
                    pauseAfterSection = .6f;
                    readoutType = 2;
                    break;
                case 9: //Credits: Ryan
                    totalCharactersToReveal = 846;
                    pauseAfterSection = 9.1f;
                    readoutType = 2;
                    break;
                case 10: //Final words
                    totalCharactersToReveal = fullCreditText.Length;
                    pauseAfterSection = 0f;
                    readoutType = 2;
                    break;
            }
            
            //Reading out the current section
            for (charactersRevealed++; charactersRevealed < totalCharactersToReveal; charactersRevealed++)
            {
                //charactersRevealed = getNextLetterIndex(fullCreditText, charactersRevealed);
                
                //Ignore spaces (to make text flow better) and avoid only taking part of a tag (to not make text screw  up)
                while (fullCreditText[charactersRevealed] == ' ' || fullCreditText[charactersRevealed] == '<')
                {
                    if(fullCreditText[charactersRevealed] == '<')
                    {
                        charactersRevealed = fullCreditText.IndexOf('>', charactersRevealed);
                    }
                    charactersRevealed++;
                }
                switch (readoutType)
                {
                    case 0: //Naive readout: No handling of multi-line blocks
                        creditText.text = fullCreditText.Substring(0, charactersRevealed);
                        break;
                    case 1: //Basic readout. Respects natural line breaks in text
                        creditText.text = fullCreditText.Substring(0, charactersRevealed) + "<alpha=#00>" + fullCreditText.Substring(charactersRevealed);
                        /*For future reference: All of this work is unnecessary - Playing with the .maxVisible[Characters/Lines/Words] variables on a
                         TextMeshPro object will do the same thing much more efficiently. You can't do the fading like that, though.*/
                        break;
                    case 2: //Fade
                        creditText.text = fullCreditText.Substring(0, charactersRevealed);

                        //This next bit of code is a clusterfuck, but I worked on it for a LONG time and it finally works and I am very tired so it stays as is.
                        
                        /*In short: It looks ahead a few characters the same way we pick the next one at the start of the loop, rendering them progressively
                          more transparently as the loop progresses. One known bug I am too exhausted to fix: There's no way to start with any less than
                          (leadingCharacters) characters rendered. The shrunken dots in the TextMeshPro are a hacky way around that, as is the backing up of
                          the charactersRevealed variable for the final message.*/
                        int leadingCharacters = 4;

                        int startingPoint = charactersRevealed-1;
                        for (int j = 0; j < Mathf.Min(leadingCharacters, totalCharactersToReveal-charactersRevealed); j++)
                        {
                            int nextCharacters = startingPoint+1;
                            while (fullCreditText[nextCharacters] == ' ' || fullCreditText[nextCharacters] == '<')
                            {
                                if (fullCreditText[nextCharacters] == '<')
                                {
                                    nextCharacters = fullCreditText.IndexOf('>', nextCharacters);
                                }
                                nextCharacters++;
                            }
                            creditText.text += fullCreditText.Substring(startingPoint+1, nextCharacters - startingPoint - 1) +"<alpha=#" + ((int)(255f * (leadingCharacters - j) / (leadingCharacters + 1f))).ToString("x") + ">"+fullCreditText[nextCharacters];
                            startingPoint = nextCharacters;
                        }
                        break;
                }
                yield return new WaitForSeconds(textSpeed);
            }

            //Putting space between sections
            yield return new WaitForSeconds(pauseAfterSection);

            //Special transitions: From epilogue into credits
            if (i == 4)
            {
                colorLerpSpeed = 0.25f;
                desiredBackgroundColor = Color.black;
                textVel = new Vector3(0, 1, 0);
                yield return new WaitForSeconds(8.6f); //Exact value TBD
            }
            //Special transitions: From credits into final sign-off
            if(i == 9)
            {
                colorLerpSpeed = 1f;
                desiredBackgroundColor = new Color(64f / 255f, 72f / 255f, 155f / 255f);
                textVel = Vector3.zero;
                creditText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 2210);
                charactersRevealed -= 4;
                yield return new WaitForSeconds(1f);
            }
        }

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
