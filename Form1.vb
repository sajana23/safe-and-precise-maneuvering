Imports System.Drawing.Text
Public Class Form1
    Private button1 As Integer
    Dim speed As Integer
    Dim collision As Integer
    Dim road(7) As PictureBox
    Dim count As Integer = 0
    Dim random As New Random()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        speed = 3
        road(0) = PictureBox1
        road(1) = PictureBox2
        road(2) = PictureBox3
        road(3) = PictureBox4
        road(4) = PictureBox5
        road(5) = PictureBox6
        road(6) = PictureBox7
        road(7) = PictureBox8
    End Sub

    Private Sub RoadMover_Tick(sender As Object, e As EventArgs) Handles RoadMover.Tick
        ' Move the road
        For x As Integer = 0 To 7
            road(x).Top += speed
            If road(x).Top >= Me.Height Then
                road(x).Top = -road(x).Height
            End If
        Next

        ' Move the cars only if there is no collision
        If Not IsCollision() Then
            MoveCar(car_1)
            MoveCar(car_2)
        End If

        ' Check for collisions
        If IsCollision() Then
            collision += 1
            Label3.Text = "Collision: " & collision

            ' Stop all cars and end game if collision count exceeds 200
            If collision > 200 Then
                StopCars()
                EndGame()   ' Display end game elements
            End If
        End If

        ' Update speed based on count
        UpdateSpeed()
    End Sub

    Private Function IsCollision() As Boolean
        ' Check if car_3 collides with car_1 or car_2
        Return car_3.Bounds.IntersectsWith(car_1.Bounds) OrElse car_3.Bounds.IntersectsWith(car_2.Bounds)
    End Function

    Private Sub StopCars()
        ' Stop all cars
        car_mover1.Stop()
        car_mover2.Stop()
        car_mover3.Stop()
    End Sub

    Private Sub MoveCar(car As PictureBox)
        car.Top += speed
        If car.Top >= Me.Height Then
            car.Top = -car.Height
            car.Left = random.Next(0, Me.Width - car.Width)
        End If
    End Sub

    Private Sub UpdateSpeed()
        ' Update speed based on the count
        If count > 10 AndAlso count < 30 Then
            speed = 5
        ElseIf count > 30 AndAlso count < 50 Then
            speed = 6
        ElseIf count > 50 AndAlso count < 70 Then
            speed = 7
        ElseIf count > 100 Then
            speed = 9
        End If
    End Sub

    Private Sub EndGame()
        ' Display end game elements
        Label3.Visible = True

        ' Stop all timers
        RoadMover.Stop()
        car_mover1.Stop()
        car_mover2.Stop()
        car_mover3.Stop()
        Rightside.Stop()
        Leftside.Stop()
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' Start moving car_3 left or right on key press
        If e.KeyCode = Keys.Right Then
            Rightside.Start()
        ElseIf e.KeyCode = Keys.Left Then
            Leftside.Start()
        End If
    End Sub

    Private Sub Rightside_Tick(sender As Object, e As EventArgs) Handles Rightside.Tick
        ' Move car_3 to the right if within bounds
        If car_3.Location.X < Me.Width - car_3.Width Then
            car_3.Left += 5
        End If
    End Sub

    Private Sub Leftside_Tick(sender As Object, e As EventArgs) Handles Leftside.Tick
        ' Move car_3 to the left if within bounds
        If car_3.Location.X > 0 Then
            car_3.Left -= 5
        End If
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        ' Stop moving car_3 on key release
        Rightside.Stop()
        Leftside.Stop()
    End Sub

    Private Sub car_mover1_Tick(sender As Object, e As EventArgs) Handles car_mover1.Tick
        ' Move car_1 and handle reset if it reaches bottom
        car_1.Top += collision / 2
        If car_1.Top >= Me.Height Then
            count += 1
            Label1.Text = "count: " & count
            car_1.Top = -(CInt(Math.Ceiling(Rnd() * 200)) + car_1.Height)
            car_1.Left = CInt(Math.Ceiling(Rnd() * (Me.Width - car_1.Width)))
        End If
    End Sub

    Private Sub car_mover2_Tick(sender As Object, e As EventArgs) Handles car_mover2.Tick
        ' Move car_2 and handle reset if it reaches bottom
        car_2.Top += collision / 3
        If car_2.Top >= Me.Height Then
            count += 1
            Label2.Text = "count: " & count
            car_2.Top = -(CInt(Math.Ceiling(Rnd() * 200)) + car_2.Height)
            car_2.Left = CInt(Math.Ceiling(Rnd() * (Me.Width - car_2.Width)))
        End If
    End Sub

    Private Sub car_mover3_Tick(sender As Object, e As EventArgs) Handles car_mover3.Tick
        ' Move car_3 unless collision is greater than 200, then stop it
        If collision > 200 Then
            car_mover3.Stop()  ' Stop moving car_3
            Label3.Text = "Collision: " & collision & " - Car 3 stopped"
        Else
            car_3.Top += collision / 3
            If car_3.Top >= Me.Height Then
                count += 1
                Label3.Text = "count: " & count
                car_3.Top = -(CInt(Math.Ceiling(Rnd() * 200)) + car_3.Height)
                car_3.Left = CInt(Math.Ceiling(Rnd() * (Me.Width - car_3.Width)))
            End If
        End If
    End Sub
End Class
