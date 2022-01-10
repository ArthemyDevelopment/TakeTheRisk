using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class Upgrade : MonoBehaviour 
{
    /*
     *Cada objeto de upgrade del menu debe tener este script, contiene toda la info necesaria sobre como y por cuanto mejorar
     * las estadisticas junto con las formulas y metodos correspondientes. La info en pantalla se actualiza cada vez se abre
     * el menu y cuando se mejora una stat.
     */
    
    enum TypeOfUpgrade //Define como se aplicara el ratio de mejora, ya sea sumando, restando o multiplicando una cantidad
    {
        sum,
        rest,
        mult
    }
    
    
    [SerializeField]private TypeOfUpgrade type_Upgrade; //Definir el tipo de mejora en cada objeto
    [SerializeField]private int I_ActLv; //Nivel actual de la mejora
    [SerializeField]private float F_RatioMejora; //Por cuanto mejorara la mejora
    [SerializeField]private Button B_BotonMejora; //Ref al boton para mejorar
    [SerializeField]private TMP_Text Tx_Lv; //Ref al texto con el nivel

    private void OnEnable()
    {

        UpdateInfo();
    }

    public void UpdateInfo() //Setear si se puede mejorar y el nivel
    {
        if (I_ActLv < 10)
        {
            B_BotonMejora.interactable = CanUpgrade();
            Tx_Lv.text = I_ActLv.ToString();
        }
        else
        {
            B_BotonMejora.gameObject.SetActive(false);
            Tx_Lv.text = "10";
        }
    }
    
    
    bool CanUpgrade() //Comprueba si hay suficientes puntos para mejorar
    {
        if (UpgradesSystem.current.I_PuntosMejora != 0)
        {
            if (UpgradesSystem.current.I_Coste <= UpgradesSystem.current.I_PuntosMejora)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    public float UpgradeStat(float f) //Se aplica el ratio de mejora segun el tipo si el valor es float, toma como parametro el valor actual y retorna el valor mejorado
    {
        float temp = 0;
        switch (type_Upgrade)
        {
            case TypeOfUpgrade.sum:
                temp = (f + F_RatioMejora);
                break;
            
            case TypeOfUpgrade.rest:
                temp = (f - F_RatioMejora);
                break;
            
            case TypeOfUpgrade.mult:
                temp = (f * F_RatioMejora);
                break;
        }
        I_ActLv++;
        return temp;

    }

    public int UpgradeStat(int i) //Se aplica el ratio de mejora segun el tipo si el valor es int, toma como parametro el valor actual y retorna el valor mejorado
    {
        int temp = 0;
        switch (type_Upgrade)
        {
            case TypeOfUpgrade.sum:
                temp = (int)(i + F_RatioMejora);
                break;
            
            case TypeOfUpgrade.rest:
                temp = (int)(i - F_RatioMejora);
                break;
            
            case TypeOfUpgrade.mult:
                temp = (int)(i * F_RatioMejora);
                break;
        }
        I_ActLv++;

        return temp;
    }


    
    
    
}
