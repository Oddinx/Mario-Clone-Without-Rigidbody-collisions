# SMB1 Physics Improvements — Pendientes

Referencia: disassembly original del ROM de Super Mario Bros (NES, 6502 ASM).
Este documento registra las mejoras de física/lógica extraídas del código original
que aún no están implementadas en el clon de Unity.

---

## ✅ Ya implementado

| Mejora | Registro ASM | Notas |
|---|---|---|
| Cooldown tras pisar Koopa (shell) | `StompTimer = $0791` | `stompCooldown = 0.25f` en `Colisionenemigo.cs` |
| Enemigos no se activan lejos de Mario | `EnemyIntervalTimer = $0796` | `Velocidadenemigo.cs`, distancia 15u |
| Fix colisión lateral entre enemigos | `SideCollisionTimer = $0785` | `Mathf.Abs` en `Goombamovement.cs` |
| Fix bounce infinito al pisar enemigo | `StompChainCounter = $0484` | `BounceActivo()` en `Player.cs` |
| Mario no se mueve al morir | `muerto = true` flag | Early return en `Update()` |
| Momentum aéreo | `Player_X_MoveForce = $0705` | `accelerationTimeAirborne = 0.4f`, contra-dirección `0.7f` |
| **InjuryTimer: invencibilidad tras daño** | **`InjuryTimer = $079e`** | **`injuryTimer = 2f` + blink en `Player.cs`** |

---

## ⏳ Pendiente de implementar

### 1. 🎮 Gravedad diferenciada: subida vs bajada
**Registro ASM:** `VerticalForce = $0709`, `VerticalForceDown = $070a`

El original aplica **más gravedad al bajar** que al subir. Esto hace que el arco del
salto sea asimétrico: rápido al caer, más "flotante" al subir.

```csharp
// ACTUAL (Player.cs):
velocity.y += gravity * Time.deltaTime; // mismo valor siempre

// OBJETIVO:
float gravityUp   = -20f; // mientras sube (menos fuerza)
float gravityDown = -30f; // mientras baja (más fuerza, cae rápido)
float appliedGravity = velocity.y > 0 ? gravityUp : gravityDown;
velocity.y += appliedGravity * Time.deltaTime;
```

**Impacto:** El salto se sentiría mucho más parecido al original.  
**Archivo:** `Player.cs` — método `Update()` / línea de `gravity`.

---

### 2. 🏃 Fricción al frenar en el suelo
**Registro ASM:** `FrictionAdderHigh = $0701`, `FrictionAdderLow = $0702`

El original aplica fricción explícita: cuando sueltas el stick, Mario desacelera
gradualmente con un valor de fricción, no con `SmoothDamp`. Corre, suelta el botón
y hay un pequeño "deslizamiento".

```csharp
// ACTUAL:
velocity.x = Mathf.SmoothDamp(...); // suavizado genérico

// OBJETIVO: si no hay input Y está en el suelo, aplicar fricción
if(Mathf.Abs(input.x) < 0.01f && controller.collisions.below) {
    velocity.x = Mathf.MoveTowards(velocity.x, 0f, frictionDecel * Time.deltaTime);
}
```

**Valores sugeridos:** `frictionDecel = 25f` (ajustar al gusto).  
**Archivo:** `Player.cs` — bloque `else` del movimiento horizontal.

---

### 3. ⭐ StarInvincibleTimer — invencibilidad de estrella
**Registro ASM:** `StarInvincibleTimer = $079f`

Cuando Mario agarra una estrella, el original usa un timer separado del `InjuryTimer`.
Durante este estado Mario puede eliminar enemigos al tocá·los (no solo al pisarlos).
Actualmente no existe en el clon.

**Qué implementar:**
- Timer de ~10 segundos al agarrar la estrella.
- Durante el timer: cualquier contacto lateral con enemigo → enemigo muere.
- Efecto visual: colores parpadeantes (palette cycling en el original).

**Archivo:** `Player.cs` + `Colisionenemigo.cs`.

---

### 4. 🐢 ShellChainCounter — combo de puntos con caparazón
**Registro ASM:** `ShellChainCounter = $0125`, `FloateyNum_Control = $0110`

Cuando un caparazón en movimiento elimina enemigos en cadena, los puntos se
multiplican: 100, 200, 400, 800, 1000, 2000... hasta 1-UP.

```
100 → 200 → 400 → 800 → 1000 → 2000 → 4000 → 5000 → 8000 → 1-UP
```

**Archivo:** `Colisionenemigo.cs` — en el bloque donde el shell colisiona con enemigos.

---

### 5. 💨 Velocidad máxima separada: caminar vs correr
**Registro ASM:** `MaximumLeftSpeed = $0450`, `MaximumRightSpeed = $0456`

El original tiene **tablas de velocidad máxima** separadas para caminar y correr,
tanto para izquierda como derecha. Actualmente `Definirvelocidad()` usa un
multiplicador simple.

```csharp
// ACTUAL:
speed *= runningMultiplyer; // 1.5x o 2x aproximado

// OBJETIVO: velocidades exactas del original (en unidades Unity aprox):
const float MAX_WALK_SPEED   = 5.5f;
const float MAX_RUN_SPEED    = 9.5f;
const float MAX_SPRINT_SPEED = 11f;  // solo si RunningTimer > threshold
```

**Archivo:** `Player.cs` — `Definirvelocidad()`.

---

### 6. 🏊 SwimmingFlag — física de nado separada
**Registro ASM:** `SwimmingFlag = $0704`

El original tiene un flag explícito que cambia completamente la física:
- Gravedad reducida en el agua.
- Cada presión de botón da un impulso vertical.
- La velocidad horizontal se reduce.

Actualmente el clon no tiene niveles de agua implementados, pero si se agregan,
esta lógica sería necesaria.

**Archivo:** `Player.cs` — nuevo estado `Nadando` en `estadosmario`.

---

### 7. ⏰ GameTimerExpiredFlag — muerte por tiempo
**Registro ASM:** `GameTimerExpiredFlag = $0759`, `GameTimerCtrlTimer = $0787`

El original tiene un timer de nivel visible en pantalla. Al llegar a 0, Mario muere.
El clon no tiene este sistema.

**Archivo:** `Manager.cs` — agregar countdown + trigger de `Death()`.

---

## Notas técnicas de conversión

El original corre en NES a 60fps con aritmética de **punto fijo de 8 bits**.
Las velocidades en el original están expresadas como píxeles/frame × 256 (fracción).

Conversión aproximada para Unity (asumiendo 1 tile ≈ 1 unidad Unity):
```
Velocidad NES (píx/frame) = valor_ASM / 256 * 60
Velocidad Unity (u/s)     = velocidad_NES * (1u / 16px) * 16
```

Por ejemplo, `MaximumRightSpeed` en el original con `Z` (run) ≈ 2.5 píx/frame ≈ `9.5f` en Unity.
