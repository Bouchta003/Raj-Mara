using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class DialogueManager : MonoBehaviour
{
    //Position in dialogue of the characters talking
    [SerializeField] public Transform leftCharacterPosition;
    [SerializeField] public Transform rightCharacterPosition;

    [SerializeField] public Image leftCharacterImage;
    [SerializeField] public Image rightCharacterImage;
    //Text files of dialogues
    [SerializeField] public string textFileName;
    [SerializeField] public TextMeshProUGUI dialogueText;

    [SerializeField] Transform dialogueWindow;
    StreamReader sr;
    string path = "Assets/Ressources/Dialogues/";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path += textFileName + ".txt";
        sr = new StreamReader(path);
        dialogueText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            string line = sr.ReadLine();
            if (line == null) return;

            if (line == "*L*")
            {
                SetImageBrightness(leftCharacterImage, 1f);   // Left: 100%
                SetImageBrightness(rightCharacterImage, 0.2f); // Right: 20%
                line = sr.ReadLine();
            }
            else if (line == "*R*")
            {
                SetImageBrightness(leftCharacterImage, 0.2f);  // Left: 20%
                SetImageBrightness(rightCharacterImage, 1f);   // Right: 100%
                line = sr.ReadLine();
            }

            dialogueText.text = line;
            Debug.Log(line);
        }
    }

    void SetImageBrightness(Image image, float value)
    {
        Color original = image.color;
        Color.RGBToHSV(original, out float h, out float s, out float v);
        v = Mathf.Clamp01(value);
        image.color = Color.HSVToRGB(h, s, v);
    }
}
