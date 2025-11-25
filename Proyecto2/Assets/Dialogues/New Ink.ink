VAR visited_port = false
VAR has_key = false

=== hombre1 ===
Hola viajero.
¿Qué haces aquí?

+ Busco al capitán.
    Está en el puerto.
    ~ visited_port = true
-> END

+ Solo paso por aquí.
    Bien, no causes problemas.
-> END


=== capitan ===
{visited_port:
    Veo que ya fuiste al puerto. Bien.
- else:
    ¿Qué haces aquí sin pasar por el puerto?
}
-> END


=== organillero ===
Te encuentras a un hombre extraño.

+ ¿Quién eres?
    Te presentas y preguntas su historia.
    ~ has_key = true
    -> END

+ Hola.
    Hola, soy un hombre del pasado.
    ~ has_key = true
-> END



=== puerta ===
{has_key:
    Usas la llave y abres la puerta.
- else:
    La puerta está cerrada, necesitas una llave.
}
-> END
