using UnityEngine;
using UnityEngine.UI;
using static Const;

public class UIPopupText : MonoBehaviour
{
    public Text normalDamageText;
    public Text critDamageText;
    public Text critSkillText;
    public Text healText;
    public Text buffText;
    public Text skillText;
    public Text petText;
    public Vector3 offset;
    public float floatTime = 0.8f;

    // 合并相关变量
    private static float lastPopupTime = 0f;
    private static float mergeTimeWindow = 0.3f; // 合并时间窗口
    private static float mergedDamage = 0f;
    private static int popupCount = 0;

    internal void InitPopup(float number, Const.SIDE side, bool isCrit, bool isSkill)
    {
        // 检查是否可以合并
        if (side!= SIDE.HEAL && CanMergeWithLastPopup())
        {
            MergeDamage(number);
        }

        // 新的飘字
        ShowPopup(number, side, isCrit, isSkill);
        UpdateLastPopupInfo(number);
    }

    private bool CanMergeWithLastPopup()
    {
        return Time.time - lastPopupTime < mergeTimeWindow;
    }

    private void MergeDamage(float number)
    {
        mergedDamage += number;
        popupCount++;

        // 更新现有飘字显示
        string mergedText = GetMergedText();

        normalDamageText.text = mergedText;
        skillText.text = mergedText;
        petText.text = mergedText;
        critDamageText.text = "CRIT" + mergedText;
        critSkillText.text = "CRIT" + mergedText;

        // 重置计时器
        lastPopupTime = Time.time;
    }

    private void ShowPopup(float number, Const.SIDE side, bool isCrit, bool isSkill)
    {
        string text = (popupCount > 1) ? GetMergedText() : number.ToString("0");

        this.normalDamageText.enabled = (side == SIDE.PLAYER && !isCrit);
        this.skillText.enabled = (side == SIDE.SKILL && !isCrit);
        this.petText.enabled = side == SIDE.PET;
        this.healText.enabled = side == SIDE.HEAL;

        this.critDamageText.enabled = (isCrit && !isSkill);
        this.critSkillText.enabled = (side == SIDE.SKILL && isCrit && isSkill);

        this.normalDamageText.text = text;
        this.skillText.text = text;
        this.petText.text = text;
        this.healText.text = "+ " + text;
        this.critDamageText.text = "CRIT" + text;
        this.critSkillText.text = "CRIT" + text;

        StartFloatAnimation(isCrit);
    }

    private void UpdateLastPopupInfo(float number)
    {
        lastPopupTime = Time.time;
        mergedDamage = number;
        popupCount = 1;
    }

    private string GetMergedText()
    {
        if (popupCount > 1)
        {

            return $"{mergedDamage:0}";
        }
        return mergedDamage.ToString("0");
    }

    private void StartFloatAnimation(bool isCrit)
    {
        float time = Random.Range(0.5f, 0.9f);
        float height = Random.Range(-0.2f, 0.2f);
        float disperse = Random.Range(0.1f, 0.3f);
        disperse += Mathf.Sign(disperse) * 0.3f;

        if (this.gameObject != null)
        {
            LeanTween.moveX(this.gameObject, this.transform.position.x + disperse, time);
            if (isCrit)
            {
                // 暴击特效
                // LeanTween.scale(this.gameObject, this.transform.position + offset, time);
                // SoundManager.Instance.PlaySound(SoundDefine.AttackStar);
            }
            LeanTween.moveY(this.gameObject, this.transform.position.y + height, time - 0.2f)
                .setEaseOutBack()
                .setOnComplete(() => {
                    // 动画完成后重置合并数据
                    if (Time.time - lastPopupTime > mergeTimeWindow)
                    {
                        ResetMergeData();
                    }
                })
                .setDestroyOnComplete(true);
        }
    }

    private void ResetMergeData()
    {
        mergedDamage = 0f;
        popupCount = 0;
    }

    void Update()
    {
        // 可以在这里添加其他更新逻辑
    }
}