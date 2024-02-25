using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Character currentCharacter;
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;
    //�ж�����ֵ�Ƿ���Ҫ�ָ�
    private bool isRecovering;

    float powerPersentage;

    public void Update()
    {
        //�����ɫѪ��С����ɫ�����ɫѪ����������
        if (healthDelayImage.fillAmount > healthImage.fillAmount) 
        {
            healthDelayImage.fillAmount -= Time.deltaTime * 0.5f;
        }
        if (isRecovering)
        {
            powerPersentage = currentCharacter.currentPower / currentCharacter.maxPower;
            powerImage.fillAmount = powerPersentage;

            if (powerPersentage >= 1)
            { 
                isRecovering = false;
                return;
            }
        }
    }

    public void OnHealthChange(float persentage)
    { 
        healthImage.fillAmount = persentage;
    }

    public void OnPowerChange(Character character)
    { 
        isRecovering = true;
        currentCharacter = character;
    }
}
