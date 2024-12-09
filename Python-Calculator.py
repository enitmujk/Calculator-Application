import json
import os
import math

def perform_basic_calculation(expression):
    try:
        if '/ 0' in expression or '/0' in expression:
            return "Error: Division by zero is not allowed."
        result = eval(expression)
        return result
    except Exception as e:
        return "Invalid input. Please enter a numeric value."
    
def save_history(expression, result):
    history = {"expression": expression, "result": result}
    file_path = "py-calc.json"

    if os.path.exists(file_path):
        try:
            with open(file_path, "r") as file:
                data = json.load(file)
        except json.JSONDecodeError:
           
            data = []
    else:
        
        data = []

    data.append(history)

    with open(file_path, "w") as file:
        json.dump(data, file, indent=4)

def view_history():
    file_path = "py-calc.json"
    
    if os.path.exists(file_path):
        try:
            with open(file_path, "r") as file:
                data = json.load(file)
                if data:
                    print("History:")
                    for i, entry in enumerate(data, start=1):
                        print(f"{i}: {entry['expression']} = {entry['result']}")
                else:
                    print("No history available.")
        except json.JSONDecodeError:
            print("No history available. The file is empty or corrupted.")
    else:
        print("No history file found.")

def calculate_square_root():
    try:
        number = float(input("Enter a number to calculate its square root: "))
        if number < 0:
            print("Error: Cannot calculate the square root of a negative number.")
            return
        result = math.sqrt(number)
        expression = f"√{number}"
        print(f"Square Root: {expression} = {result}")

        save_history(expression, result)
    except ValueError:
        print("Invalid input. Please enter a numeric value.")

def main():
    while True:
        print("Select operation:")
        print("1. Perform Basic Calculation")
        print("2. Calculate Square Root")
        print("3. View Calculation History")
        print("4. Exit")

        choice = input("Enter choice (1/2/3/4): ")
        print(f"User chose: {choice}")

        if choice == '1':
            expression = input("Enter your calculation (addition/subtraction/multiplication/division): ")
            result = perform_basic_calculation(expression)
            print(f"Result: {result}")

            save_history(expression, result)

        elif choice == '2':
            calculate_square_root()

        elif choice == '3':
            view_history()

        elif choice == '4':
            print("Exiting the calculator. Goodbye!")
            break

        else:
            print("Invalid input. Please choose 1, 2, 3, or 4.")

if __name__ == "__main__":
    main()