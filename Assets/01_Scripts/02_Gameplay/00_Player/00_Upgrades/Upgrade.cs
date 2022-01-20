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
    
    

    [SerializeField]private int I_ActLv; //Nivel actual de la mejora
    [SerializeField]private float F_PrevValueScale; //Aumento de mejora en base al valor anterior
    [SerializeField]private float F_LvValueScale; //Aumento de mejora en base al nivel
    [SerializeField]private float F_FlatSumScale; //Aumento de mejora en base a un valor plano
    [SerializeField]private Button B_BotonMejora; //Ref al boton para mejorar
    [SerializeField]private TMP_Text Tx_Lv; //Ref al texto con el nivel

    private void OnEnable()
    {

        UpdateInfo();
    }

    public void UpdateInfo() //Setear si se puede mejorar y el nivel
    {
        if (I_ActLv < 20)
        {
            B_BotonMejora.interactable = CanUpgrade();
            Tx_Lv.text = I_ActLv.ToString();
        }
        else
        {
            B_BotonMejora.gameObject.SetActive(false);
            Tx_Lv.text = "20";
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
        I_ActLv++;
        
        temp = (f * F_PrevValueScale) + (I_ActLv * F_LvValueScale) + F_FlatSumScale;
        
        return temp;

    }

    public int UpgradeStat(int i) //Se aplica el ratio de mejora segun el tipo si el valor es int, toma como parametro el valor actual y retorna el valor mejorado
    {
        int temp = 0;
        I_ActLv++;

        temp = Mathf.RoundToInt(i * F_PrevValueScale) + (int)(I_ActLv * F_LvValueScale) + (int)F_FlatSumScale;
        
        return temp;
    }


    
    
    
}
