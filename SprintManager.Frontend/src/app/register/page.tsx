"use client"

import React, { useState } from 'react';
import Form from 'next/form'
import { RegisterRequest } from '../types/auth';

export default function Register() {

    const [name, setName] = useState("");
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const registerData = async () => {
        const request: RegisterRequest = {
            name: name,
            username: username,
            email: email,
            password: password
        }

        const response = await fetch('https://localhost:7060/api/Auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(request),
        });

            const result = await response.json();
            console.log(result);
    };

    console.log(name);
    console.log(username);
    console.log(email);
    console.log(password);

    return (
        <div className='flex min-h-screen justify-center items-center'>
            <Form style={{border: '1px solid red'}} className='flex flex-col w-125' action="">
                <label>Name:</label>
                <input 
                    className='border black' 
                    name="name" 
                    onChange={(event) => setName(event.target.value)}
                />
                
                <label>Username:</label>
                <input 
                    className='border black' 
                    name="username" 
                    onChange={(event) => setUsername(event.target.value)}
                />

                <label>Email:</label>
                <input 
                    className='border black' 
                    name="email"
                    onChange={(event) => setEmail(event.target.value)}
                />

                <label>Password:</label>
                <input 
                    className='border black' 
                    name="password" 
                    type='password'
                    onChange={(event) => setPassword(event.target.value)}
                />
                
                <button onClick={registerData} type="submit">Submit</button>
            </Form>
        </div>
    )
}