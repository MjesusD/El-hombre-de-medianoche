=== organillero ===
¿Quién eres?
¿Qué haces aquí?

Soy un capitán, y viajé a través del tiempo.
    Interesante.
    Entonces puedes ayudarme.
    ¿Qué necesitas?
    Como ves soy un organillero, y he perdido una pieza de mi querido amigo.
    Sin ella no puedo seguir haciendo mi trabajo. 
    De seguro disfrutabas de la música cuando niño, 
    y hay muchos que han quedado sin ella en esta ciudad.
    Entonces, ¿aceptas ayudarme a encontrala?

    + Si
        Ja ja ja! Sabía que vendría alguien en mi ayuda.
        Toma esa nota que hay más adelante. Te será de ayuda.
        ¡Buena suerte!

    ->END

    + No
        Hey! No seas así. 
        Ambos necesitamos algo.
        Tienes que hacerlo para poder volver a donde estabas.
        Así que...¿Es un trato?
    Trato.
    Eso es.
    Toma esa nota que hay más adelante, te será de ayuda.
    
-> END



+ Debería irme.
    Bien, hasta pronto.
-> END

=== organillero_repeat ===
Ya hablamos antes, viajero.
¿Necesitas algo más?
-> END


=== organillero2 ===
¿Pudiste encontrar la pieza faltanate de mi amigo?
    +Si
Sabía que podías! 
¿Puedes dármelo?
#darObjeto

-> END
    +No
   No te preocupes, sigue buscando.
-> END


=== organillero2_repeat ===
¡Recuperaste mi accesorio!
¿Puedes dármelo?
#darObjeto
Ahora podré tocar de nuevo con mi amigo. (Risas)
    ¿Te gustaría escuchar?
+ Si.
#panel_Audio1
    (Empieza a sonar una música y la gente se acerca)
Bueno, no hay tiempo que perder. 
¡Vamos!
->END
+ No
    Pues entonces, otro día será.
    En marcha!
-> END
