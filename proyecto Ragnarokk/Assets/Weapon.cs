using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//luego las armas deben tener mas caracteristicas.
[CreateAssetMenu(menuName = "Combat/Weapon")]
public class Weapon : Item
{
    public int BaseDamage = 50;

    //deberia tener la info de:

    //Los estados que aplica.
    public List<CombatState> ListaDeEstadosQueAplica;

    //Tipo
    public CombatType TipoDeDañoQueAplica;

}
