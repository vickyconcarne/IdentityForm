using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using AdVd.GlyphRecognition;
using TMPro;

public class CCastDrawnGlyph : MonoBehaviour
{

    #region Private Variables
    private GlyphDrawInput m_Gdi;
    [SerializeField]
    private GlyphSet m_CustomDictionaryGlyphSet;

    [Header("Training Mode")]
    [SerializeField]
    Image m_GlyphInputBackground = null;


    [SerializeField]
    private GameObject m_SliderGlyphDisplay;
    [SerializeField]
    private TextMeshProUGUI m_SliderText;
    [SerializeField]
    private GameObject m_SliderObject;

    [Header("Clear drawing")]
    [SerializeField]
    private Button b_ClearDrawingsButton;

    #endregion



    // Use this for initialization
    void Start () {
        m_Gdi = this.GetComponent<GlyphDrawInput>();
	}

	
	public void OnGlyphCast(int index, GlyphMatch match)
    {
        if (match != null)
        {
            
            //m_SliderGlyphDisplay.GetComponent<GlyphDisplay>().glyph = match.glyph;
            m_SliderObject.GetComponent<Animator>().SetTrigger("ShowNotif");
            m_SliderText.text = "Added " + match.target.name;
            /*if (m_Training)
            {
                if(m_ThisNotificationBox) m_ThisNotificationBox.PopUpNotification("Detected letter: " + match.target.name);
            }*/
            /*b_ClearDrawingsButton.onClick.Invoke(); //Call the button that clears inputs and drawings
            m_GlyphDisplay.GetComponent<GlyphDisplay>().glyph = CDictionaryManager.Instance.GetCustomGlyph(m_alphabetDictionary[match.target.name]);
            m_GlyphDisplay.SetActive(true);*/

        }
        else
        {
            Debug.Log("No match");
            /*if (m_Training)
            {
                if (m_ThisNotificationBox) m_ThisNotificationBox.PopUpNotification("No match");
            }*/
        }
    }

    public void CastOrRecast()
    {
        float start = Time.realtimeSinceStartup;
        if (!m_Gdi.Cast()) if (m_Gdi.castedGlyph != null) m_Gdi.Cast(m_Gdi.castedGlyph);
        Debug.Log("Cast time: " + (Time.realtimeSinceStartup - start));
    }

    public Glyph GetCustomGlyph(int glyph_index)
    {
        return m_CustomDictionaryGlyphSet[glyph_index];
    }
}
