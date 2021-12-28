using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonUpgrade : MonoBehaviour
{
    public GameObject capsule;
    public GameObject textMesh;
    private float targetAlpha;
    private bool isActive;
    private float fadeSpeed;
    private SpriteRenderer capsuleComp;
    private TextMeshPro textComp;
    // Start is called before the first frame update
    void Start()
    {
        capsuleComp = capsule.GetComponent<SpriteRenderer>();
        textComp = textMesh.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        Color capsuleColor = capsuleComp.color;
        Color textColor = textComp.color;
        if (capsuleColor.a > targetAlpha)
        {
            capsuleColor.a -= Time.deltaTime * fadeSpeed;
            if (capsuleColor.a < targetAlpha) capsuleColor.a = targetAlpha;
        }
        if (capsuleColor.a < targetAlpha)
        {
            capsuleColor.a += Time.deltaTime * fadeSpeed;
            if (capsuleColor.a > targetAlpha) capsuleColor.a = targetAlpha;
        }
        textColor.a = capsuleColor.a;
        capsuleComp.color = capsuleColor;
        textComp.color = textColor;
    }

    public void Hide(float speed)
    {
        targetAlpha = 0;
        isActive = false;
        fadeSpeed = speed;
    }

    public void Show(float speed)
    {
        targetAlpha = 1;
        isActive = true;
        fadeSpeed = speed;
    }

    private void OnMouseDown()
    {
        if (isActive)
        {
            GameManager.Instance.AdvanceLevel();
            SoundFX.Instance.PlaySoundFX("upgrade");
        }
    }
}
