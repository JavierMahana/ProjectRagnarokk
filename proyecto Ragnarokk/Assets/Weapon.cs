using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//luego las armas deben tener mas caracteristicas.
[CreateAssetMenu(menuName = "Combat/Weapon")]
public class Weapon : ScriptableObject
{
    public string Name = "unnamed weapon";
    public int BaseDamage = 50;
    public Sprite sprite;

    //deberia tener la info de:

    //Los estados que aplica.
    public List<CombatState> ListaDeEstadosQueAplica;

    //Tipo
    public CombatType TipoDeDañoQueAplica;

}
