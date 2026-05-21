temporal=True

while temporal==True:

    x=int(input("Enter a number: "))
    y=int(input("Enter another number: "))
    opc=int(input("1 = restar - 2 = multiplicar - 3 = dividir - 4 = sumar"))
    if opc==1:
        print(x-y)
    elif opc==2:
        print(x*y)
    elif opc==3:
        print(x/y)
    elif opc==4:
        print(x+y)
    else:    print("Invalid option")

    t=int(input("Do you want to continue? 1 = yes"))
    if t==1:
        temporal=True
    else:
        temporal=False
