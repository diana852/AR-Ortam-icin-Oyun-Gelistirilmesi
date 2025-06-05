using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardId;
    public Material frontMaterial;
    public Material backMaterial;

    private bool isFlipped = false;
    private bool isMatched = false;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();

    }

    public void SetupCard(int id, Material front, Material back)
    {
        cardId = id;
        frontMaterial = front;
        backMaterial = back;

        rend = GetComponent<Renderer>();
        rend.material = backMaterial; // hemen arka materyali uygula

        isFlipped = false;

        ShowBack();
    }

    public void OnMouseDown()
    {
        if (isFlipped || isMatched) return;
        MemoryCardManager.Instance.OnCardClicked(this);
    }

    public void ShowFront()
    {
        if (!isFlipped)
            StartCoroutine(FlipAnimation(true));
    }

    public void ShowBack()
    {
        if (isFlipped)
            StartCoroutine(FlipAnimation(false));
    }

    private System.Collections.IEnumerator FlipAnimation(bool showFront)
    {
        float duration = 0.3f;
        float time = 0f;
        float startAngle = transform.rotation.eulerAngles.y;
        float endAngle = startAngle + 180f;

        while (time < duration)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, time / duration);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            time += Time.deltaTime;
            yield return null;
        }

        // Yüzü değiştirme tam ortadayken olsun
        if (showFront)
            rend.material = frontMaterial;
        else
            rend.material = backMaterial;

        transform.rotation = Quaternion.Euler(0, endAngle, 0);

        isFlipped = showFront;
    }


    public void SetMatched()
    {
        isMatched = true;
    }
}