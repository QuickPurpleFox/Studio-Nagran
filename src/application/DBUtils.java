package application;

import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import javafx.event.ActionEvent;
import javafx.fxml.FXMLLoader;
import javafx.scene.Node;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.stage.Stage;

public class DBUtils {

	public static void changeScene(ActionEvent event, String fxmlFile, String title, String username) {
		Parent root = null;
		
		if(username != null) {
			try {
				FXMLLoader loader = new FXMLLoader(DBUtils.class.getResource(fxmlFile));
				root = loader.load();
				LoggedInController loggedInController = loader.getController();
				loggedInController.setUserInformation(username);
			}catch (IOException e) {
				e.printStackTrace();
			}
		}else {
			try {
				root = FXMLLoader.load(DBUtils.class.getResource(fxmlFile));
			}catch (IOException e) {
				e.printStackTrace();
			}
		}
		Stage stage = (Stage) ((Node) event.getSource()).getScene().getWindow();
		stage.setTitle(title);
		stage.setScene(new Scene(root, 600, 400));
		stage.show();
	}
	public static void signUpUser(ActionEvent event, String username, String password){
		Connection connection = null;
		PreparedStatement psInsert = null;
		PreparedStatement psCheckUserExist = null;
		ResultSet resultSet = null;
		
		try {
			connection = DriverManager.getConnection("jdbc:mysql://localhost:3306/javafx-video", "root", "root");
			psCheckUserExist = connection.prepareStatement("SELECT * FROM users WHERE username = ?");
			psCheckUserExist.setString(1, username);
			resultSet = psCheckUserExist.executeQuery();
			
			if(resultSet.isBeforeFirst()){
				System.out.println("User already exist");
				Alert alert = new Alert(Alert.AlertType.ERROR);
				alert.setContentText("You cannot use this username, ");
				alert.show();
			}
			else {
				psInsert = connection.prepareStatement("INSERT INTO users (username, password VALUES (?, ?)");
				psInsert.setString(1, username);
				psInsert.setString(2, password);
				psInsert.executeUpdate();
				
				changeScene(event, "logged-in.fxml", "Welcome", username);
			}
		}catch (SQLException e) {
			e.printStackTrace();
		}finally {
			if(resultSet != null) {
				try {
					resultSet.close();
				}catch(SQLException e) {
					e.printStackTrace();
				}
			}
			if(psCheckUserExist != null) {
				try {
					psCheckUserExist.close();
				}catch(SQLException e) {
					e.printStackTrace();
				}
			}
			if(psInsert != null) {
				try {
					psInsert.close();
				}catch(SQLException e) {
					e.printStackTrace();
				}
			}
			if(connection != null) {
				try {
					connection.close();
				}catch(SQLException e) {
					e.printStackTrace();
				}
			}
		}
	}
	
	public static void logInUser(ActionEvent event, String username, String password) {
		Connection connection = null;
		PreparedStatement preparedStatement = null;
		ResultSet resultSet = null;
		try {
			connection = DriverManager.getConnection("jdbc:mysql://localhost:3306/javafx-video", "root", "root");
			preparedStatement = connection.prepareStatement("SELECT password, FROM users WHERE username = ?");
			preparedStatement.setString(1, username);
			resultSet = preparedStatement.executeQuery();
			
			if(!resultSet.isBeforeFirst()) {
				System.out.println("User not found in the database!");
				Alert alert = new Alert(Alert.AlertType.ERROR);
				alert.setContentText("Provided credentials are incorect!");
				alert.show();
			}else {
				while (resultSet.next()) {
					String retrievedPassword = resultSet.getString("password");
					if (retrievedPassword.equals(password)) {
						changeScene(event, "logged-in.fxml", "Welcome", username);
					}else {
						System.out.println("Password did not match!");
						Alert alert = new Alert(Alert.AlertType.ERROR);
						alert.setContentText("Provided credentials are incorect!");
						alert.show();
					}
				}
			}
		}catch (SQLException e) {
			e.printStackTrace();
		}finally {
			if(resultSet != null) {
				try {
					resultSet.close();
				}catch(SQLException e) {
					e.printStackTrace();
				}
			}
			if(preparedStatement != null) {
				try {
					preparedStatement.close();
				}catch(SQLException e) {
					e.printStackTrace();
				}
			}
			if(connection != null) {
				try {
					connection.close();
				}catch(SQLException e) {
					e.printStackTrace();
				}
			}
		}
	}
}













