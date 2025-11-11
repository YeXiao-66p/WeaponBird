using UnityEngine;

public class SkillUp : MonoBehaviour
{
    public Player player;
    public PetFollow pet;
    public int maxCnt = 4;
    public int curSpeedCnt = 0;
    public int curCountPerRoundCnt = 0;
    public int curInvincibleDurationCnt = 0;
    public int curAttackCnt = 0;
    public int curSkillAttackCnt = 0;

    public void BulSpeed()
    {
        if (curSpeedCnt <= maxCnt)
        {
            curSpeedCnt += 1;
            player.fireRate += 0.1f;
            UpdateUINotice(true);
            return;
        }
            
        UpdateUINotice(false);
    }
    public void BulCountPerRound()
    {
        if (curCountPerRoundCnt <= maxCnt)
        {
            curCountPerRoundCnt += 1;
            player.bulletsPerShot += 2;
            UpdateUINotice(true);
            return;
        }

        UpdateUINotice(false);
    }
    public void InvincibleDuration()
    {
        if (curInvincibleDurationCnt <= maxCnt)
        {
            curInvincibleDurationCnt += 1;
            player.invincibleDuration += 0.5f;
            UpdateUINotice(true);
            return;
        }
        UpdateUINotice(false);
    }
    public void BulAttack()
    {
        if (curAttackCnt <= maxCnt)
        {
            curAttackCnt += 1;
            player.power += 1;
            UpdateUINotice(true);
            return;
        }
        UpdateUINotice(false);
    }
    public void SkillAttack()
    {
        if (curSkillAttackCnt <= maxCnt)
        {
            curSkillAttackCnt += 1;
            player.skillAttack += 10;
            UpdateUINotice(true);
            return;
        }
        UpdateUINotice(false);
    }
    public void UpdateUINotice(bool isSuc)
    {
        if (isSuc)
        {
            var msg = MessageBox.Show(null, string.Format("升级成功！"), null, MessageBoxType.Information);
        }
        else
        {
            var msg = MessageBox.Show(null, string.Format("已经升至最大咯！"), null, MessageBoxType.Information);
        }
    }
}
